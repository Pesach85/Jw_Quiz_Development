# JW Quiz — Architecture

Original educational game: **Bible-themed rebus + quiz + adventure**.  
Inspired by the *style* of youth activities on [jw.org — Fai vivere il racconto!](https://www.jw.org/it/cosa-dice-la-Bibbia/ragazzi/fai-vivere-il-racconto/) (immerse → reflect → learn). **Not** an official Watch Tower product. Do not copy JW.org articles, PDFs, or artwork.

## Purpose (one product, three surfaces)

| Surface | Path | Role |
|---------|------|------|
| **Web immersive** | `webapp/index.html` → [jwquiz.pages.dev](https://jwquiz.pages.dev/) | Primary player experience: 3 modes, 3D, audio |
| **Web editor** | `webapp/classic.html` | Create episodes, admin stats, shared KV/R2 |
| **Desktop** | `Jw_Quiz_Development.exe` (WinForms net472) | Projection / host mode: same 18-story rebus, XP, editor |
| **Android** | `android/` WebView | Packaged copy of `webapp/` after `python tools/sync_all.py` |

Same catalog (episodes 1–18), same PNG **keys**, same anti-spoiler rules, same star formula (3 / 2 / 1 from helps).

## Three original modes (do not collapse)

| Mode | Learning move | Flow |
|------|----------------|------|
| **Quiz** | Retrieval practice | Intro → MCQ → moral |
| **Rebus 3D** | Dual coding (image + language) | Intro → floating PNG rebus → moral |
| **Avventura** | Interleaving + spacing in one session | Intro → rebus → MCQ → moral |

Psychology used (not JW.org text): curiosity gap, progressive disclosure, immediate feedback, mastery stars, retrieval after encoding.

## Source of truth

| Data | Canonical | Copies |
|------|-----------|--------|
| Rebus PNG keys + files | `Resources/*.png` (desktop resx) | `webapp/assets/` via apply/sync |
| Story rebus fields | `StoryLibrary.cs` | `webapp/stories.js` (keep IDs/keys in sync) |
| Immersive Q&A | `webapp/index.html` `STORIES` | Original copy; titles/themes aligned to catalog |
| Photorealistic masters | `tools/photo_masters/` (local, gitignored) | Applied by `tools/apply_photo_assets.py` |
| Android www | **Generated** | `python tools/sync_android_www.py` — not committed |

## Pipeline (one command)

```powershell
python tools/sync_all.py
```

1. Apply photo masters → `Resources/` + `webapp/assets/` (if masters present)
2. Sync `webapp/` → `android/app/src/main/assets/www/`

## Copyright / content rules

- Original questions, captions, morals, and generated photorealistic plates.
- Scripture *references* and short quotes in TNM style already in-app may stay; do not paste JW.org article bodies.
- Footer must keep the unofficial disclaimer + link to jw.org.

## Deploy & build (canonical, from repo root)

All commands below assume cwd = repository root (`D:\Jw_Quiz_Development`). Do not run `tools/*.py` from `android/`.

| Surface | Command | Result |
|---------|---------|--------|
| Web preview | `cd webapp` then `python -m http.server 8080` | Player `/` · editor `/classic.html` |
| Web production | `npx wrangler pages deploy webapp --project-name=jwquiz` | [jwquiz.pages.dev](https://jwquiz.pages.dev/) — `wrangler.toml` `pages_build_output_dir = "webapp"` |
| Desktop Debug | MSBuild `Jw_Quiz_Development.csproj /p:Configuration=Debug` | `bin\Debug\Jw_Quiz_Development.exe` |
| Desktop Release | `.\build.bat` | `bin\Release\Jw_Quiz_Development.exe` |
| Assets + Android www | `python tools/sync_all.py` | Apply photo masters if present; copy `webapp/` → `android/app/src/main/assets/www/` (gitignored; restores `.gitkeep`) |
| Android APK | Open folder `android/` in Android Studio after sync | Gradle output under `android/app/build/` (do not commit) |

Working tree must be clean before Wrangler deploy (no `--commit-dirty` as default).

## Rebus stage (player)

The theater middle row (`1fr`) owns the rounded viewport. Eight plates (5 visible + 2 hidden + 1 hint) are a **4×2 board**. WebGL camera distance is computed from the board AABB (`fitRebusCamera`) so plates are never clipped by the frame. CSS fallback uses a fluid 4×2 grid (2×4 under 560px). Do not restore magic `camera.z = 6.4` or a tall cylindrical layout.
