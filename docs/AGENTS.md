# JW Quiz — Agent operating manual

Read `.github/KB.md` first. Then this file. Then `docs/ARCHITECTURE.md`.

## Product invariants

1. Three selectable modes: **Quiz**, **Rebus 3D**, **Avventura**. Never merge them into one forced path.
2. Anti-spoiler: hide title/scripture until solution (rebus). Grid tiles show episode + theme, not title.
3. PNG keys in story data, never Unicode emoji in `VisibleEmojis` / `hiddenKeys`.
4. Italian is source of truth; English is a complete parallel.
5. No JW.org copyrighted article/PDF/artwork dump. Original didactic content only.
6. Desktop stays WinForms net472. Do not add net5+ APIs.
7. Web immersive is the player UX; `classic.html` is editor/admin only.
8. Android `assets/www` is generated. Edit `webapp/`, then `python tools/sync_all.py` **from repo root** (not `android/`). The sync must restore `www/.gitkeep` so git stays clean (otherwise Wrangler warns `--commit-dirty`).

## Where to change what

| Intent | Files |
|--------|--------|
| New episode (rebus data) | `StoryLibrary.cs` + `webapp/stories.js` + Q&A block in `webapp/index.html` |
| New PNG concept | `tools/photo_concepts.py` → generate master → `python tools/sync_all.py` |
| Player UX / 3D / modes | `webapp/index.html` only |
| Editor / Cloudflare API | `webapp/classic.html`, `webapp/app.js`, `functions/api/*` |
| Desktop rebus UI | `DynamicStoryForm.cs`, `AppText.cs` |
| i18n web classic | `webapp/story-i18n.js` |
| i18n immersive | `UI.it` / `UI.en` in `webapp/index.html` |
| i18n desktop | `AppText.cs`, `StoryLocalizationService` |
| Agent protocol | `.github/skills/jw-quiz-workflow/SKILL.md`, this file, KB §10–11 |

## Stop and ask the human only when

- Changing scripture quote wording (doctrinal/accuracy review)
- Publishing/deploy credentials (`ADMIN_SECRET`, Cloudflare login)
- Adding a 19th story theme they did not request
- Destructive git (`push --force`, reset --hard)

Everything else: implement, build, update KB, commit.

## Validation

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" Jw_Quiz_Development.csproj /p:Configuration=Debug /nologo /verbosity:quiet
python tools/sync_all.py
```

Desktop exit code must be 0 before commit.

## Do not commit

- `android/.gradle`, `android/build`, `android/app/src/main/assets/www`
- `tools/photo_masters/*.png` (regenerable; applied copies live in `Resources/` and `webapp/assets/`)
- `bin/`, `obj/`, `.wrangler/`
