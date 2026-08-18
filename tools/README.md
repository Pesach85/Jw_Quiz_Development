# Tools — creative + sync pipeline

| Script | When |
|--------|------|
| `photo_concepts.py` | Map concept → prompt → rebus PNG keys |
| `apply_photo_assets.py` | Copy masters → `Resources/` + `webapp/assets/` |
| `sync_android_www.py` | Copy `webapp/` → Android WebView bundle |
| `sync_all.py` | Run both apply (if masters exist) and Android sync |

```powershell
python tools/sync_all.py
```

Photorealistic masters live in `tools/photo_masters/` (gitignored). If missing, existing `Resources/*.png` remain the deployed art.
