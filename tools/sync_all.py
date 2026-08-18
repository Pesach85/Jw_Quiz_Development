# -*- coding: utf-8 -*-
"""Single pipeline: photo apply (optional) + Android www sync."""
from __future__ import annotations
import runpy
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
MASTERS = ROOT / "tools" / "photo_masters"


def main() -> None:
    has_masters = MASTERS.exists() and any(MASTERS.glob("*.png"))
    if has_masters:
        print("== apply_photo_assets ==")
        runpy.run_path(str(ROOT / "tools" / "apply_photo_assets.py"), run_name="__main__")
    else:
        print("skip apply_photo_assets (no tools/photo_masters/*.png)")
    print("== sync_android_www ==")
    runpy.run_path(str(ROOT / "tools" / "sync_android_www.py"), run_name="__main__")
    print("sync_all done")


if __name__ == "__main__":
    main()
