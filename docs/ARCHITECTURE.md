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

## Deploy

- Cloudflare Pages: `wrangler.toml` `pages_build_output_dir = "webapp"`
- Desktop: `.\build.bat` or MSBuild Debug/Release
- Android: Android Studio on `android/` after sync
