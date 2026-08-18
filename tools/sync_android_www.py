# -*- coding: utf-8 -*-
"""Sync webapp/ into android assets/www for offline WebView packaging."""
from __future__ import annotations
import shutil
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
SRC = ROOT / "webapp"
DST = ROOT / "android" / "app" / "src" / "main" / "assets" / "www"

SKIP_NAMES = {".git", "__pycache__", "_list_keys.py", "_unique_keys.txt", "_patch_modes.py"}


def main() -> None:
    if DST.exists():
        shutil.rmtree(DST)
    DST.mkdir(parents=True, exist_ok=True)

    for path in SRC.rglob("*"):
        if any(part in SKIP_NAMES for part in path.parts):
            continue
        rel = path.relative_to(SRC)
        target = DST / rel
        if path.is_dir():
            target.mkdir(parents=True, exist_ok=True)
        else:
            target.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(path, target)
    print(f"synced {SRC} -> {DST}")


if __name__ == "__main__":
    main()
