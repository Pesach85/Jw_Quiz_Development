# -*- coding: utf-8 -*-
"""Single pipeline: photo apply (optional) + story artifacts + Android www sync."""
from __future__ import annotations

import argparse
import runpy
import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
MASTERS = ROOT / "tools" / "photo_masters"
GENERATE_SCRIPT = ROOT / "tools" / "generate_story_artifacts.py"


def run_generate_story_artifacts() -> None:
    print("== generate_story_artifacts ==")
    result = subprocess.run(
        [sys.executable, str(GENERATE_SCRIPT)],
        cwd=str(ROOT),
        check=False,
    )
    if result.returncode != 0:
        print(f"generate_story_artifacts failed (exit {result.returncode})", file=sys.stderr)
        sys.exit(result.returncode)


def main() -> None:
    parser = argparse.ArgumentParser(description="JW Quiz asset + story + Android www pipeline")
    parser.add_argument(
        "--skip-generate",
        action="store_true",
        help="Skip generate_story_artifacts.py (use existing stories.js / StoryLibrary.cs)",
    )
    args = parser.parse_args()

    has_masters = MASTERS.exists() and any(MASTERS.glob("*.png"))
    if has_masters:
        print("== apply_photo_assets ==")
        runpy.run_path(str(ROOT / "tools" / "apply_photo_assets.py"), run_name="__main__")
    else:
        print("skip apply_photo_assets (no tools/photo_masters/*.png)")

    if args.skip_generate:
        print("skip generate_story_artifacts (--skip-generate)")
    else:
        run_generate_story_artifacts()

    print("== sync_android_www ==")
    runpy.run_path(str(ROOT / "tools" / "sync_android_www.py"), run_name="__main__")
    print("sync_all done")


if __name__ == "__main__":
    main()
