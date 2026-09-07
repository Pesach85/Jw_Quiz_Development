# Tools — creative + sync pipeline

Run from **repo root** only (`D:\Jw_Quiz_Development`).

| Script | When |
|--------|------|
| `photo_concepts.py` | Map concept → prompt → rebus PNG keys |
| `apply_photo_assets.py` | Copy masters → `Resources/` + `webapp/assets/` |
| `sync_android_www.py` | Copy `webapp/` → Android WebView bundle (restores `www/.gitkeep`) |
| `sync_all.py` | **Canonical:** apply (if masters exist) + Android sync |

```powershell
python tools/sync_all.py
```

Do not run these from `android/` (scripts resolve paths from `tools/` → repo root, but muscle memory `cd android` then `python tools/...` fails).

Photorealistic masters live in `tools/photo_masters/` (gitignored). If missing, existing `Resources/*.png` remain the deployed art.

Related: desktop `.\build.bat` (Release), Wrangler `npx wrangler pages deploy webapp --project-name=jwquiz`, Android Studio on folder `android/` after sync. Full table: `docs/ARCHITECTURE.md` and `.github/KB.md` §1.
