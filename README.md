# JW Quiz Development

Educational Bible quiz: **Quiz**, **Rebus 3D**, **Avventura**.  
Web + desktop (WinForms) + Android WebView. Inspired by the *method* of [jw.org youth “Fai vivere il racconto!”](https://www.jw.org/it/cosa-dice-la-Bibbia/ragazzi/fai-vivere-il-racconto/) — not an official Watch Tower product.

## Docs

| Audience | File |
|----------|------|
| Humans (architecture) | [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) |
| Agents | [docs/AGENTS.md](docs/AGENTS.md) · [.github/KB.md](.github/KB.md) |
| Tools pipeline | [tools/README.md](tools/README.md) |
| Android | [android/README.md](android/README.md) |

## Quick start

```powershell
# Web (player)
cd webapp
python -m http.server 8080
# http://127.0.0.1:8080/          immersive 3 modes
# http://127.0.0.1:8080/classic.html   editor / admin

# Desktop
.\build.bat
# bin\Debug\Jw_Quiz_Development.exe

# Sync Android bundle from webapp
python tools/sync_all.py
```

Deploy web: `npx wrangler pages deploy webapp --project-name=jwquiz` → https://jwquiz.pages.dev/

## Stack

- Desktop: C# / .NET Framework 4.7.2 / WinForms
- Web: static `webapp/` + Cloudflare Pages Functions (KV + R2)
- Android: WebView shell over generated `assets/www`
