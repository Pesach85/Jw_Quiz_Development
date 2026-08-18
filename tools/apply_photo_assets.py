# -*- coding: utf-8 -*-
"""Apply photorealistic masters to Resources/ and webapp/assets/ by rebus key alias."""
from __future__ import annotations
import shutil
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT))
from tools.photo_concepts import CONCEPTS

MASTERS = ROOT / "tools" / "photo_masters"
RES = ROOT / "Resources"
WEB = ROOT / "webapp" / "assets"


def main() -> None:
    missing = []
    copied = 0
    for concept, (stem, _prompt, aliases) in CONCEPTS.items():
        src = MASTERS / f"{stem}.png"
        if not src.exists():
            missing.append(stem)
            continue
        for key in aliases:
            for dest_dir in (RES, WEB):
                dest_dir.mkdir(parents=True, exist_ok=True)
                shutil.copy2(src, dest_dir / f"{key}.png")
                copied += 1
    print(f"copied_slots={copied}")
    print(f"missing_masters={len(missing)}")
    if missing:
        print("missing:", ", ".join(missing))


if __name__ == "__main__":
    main()
