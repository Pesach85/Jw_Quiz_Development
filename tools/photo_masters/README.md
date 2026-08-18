# Photo masters (local)

PNG concept masters (tree.png, sheep.png, …) used by `apply_photo_assets.py`.

This folder is **gitignored**. Applied copies are stored as rebus keys in:

- `Resources/<key>.png` (desktop)
- `webapp/assets/<key>.png` (web / Pages)

To regenerate: create masters here matching `photo_concepts.py` stems, then `python tools/sync_all.py`.
