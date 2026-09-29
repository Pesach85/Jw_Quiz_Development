# JW Quiz — Agent operating manual

Read `.github/KB.md` first. Then this file. Then `docs/ARCHITECTURE.md`.

## Dataset (G1) — after reading KB

- If you modify `data/episodes.json`:
  1. `python tools/validate_episodes.py data/episodes.json --full-catalog`
  2. `python tools/sync_all.py`
  3. `python tools/verify_episode_parity.py`
- If parity fails → **do not commit**; fix data or re-sync.
- If you modify `tools/generate_story_artifacts.py` → run parity + `python tools/generate_story_artifacts.py --check` (idempotency).
- `webapp/app.js` is **off-limits** unless explicit human OK **and** scope limited to the classic flat rebus block (post F3C-A).
- Dev server: plain `python -m http.server` may serve `.js` as `text/plain` and block ES modules → use **:8081** with MIME `text/javascript` (custom handler) or `npx serve`.

## Product invariants

1. Three selectable modes: **Quiz**, **Rebus 3D**, **Avventura**. Never merge them into one forced path.
2. Anti-spoiler: hide title/scripture until solution (rebus). Grid tiles show episode + theme, not title.
3. PNG keys in story data, never Unicode emoji in `VisibleEmojis` / `hiddenKeys`.
4. Italian is source of truth; English is a complete parallel.
5. No JW.org copyrighted article/PDF/artwork dump. Original didactic content only.
6. Desktop stays WinForms net472. Do not add net5+ APIs.
7. Web immersive is the player UX; `classic.html` is editor/admin only.
8. Android `assets/www` is generated. Edit `webapp/`, then `python tools/sync_all.py` **from repo root** (not `android/`). The sync must restore `www/.gitkeep` so git stays clean (otherwise Wrangler warns `--commit-dirty`).
9. Episode canon is `data/episodes.json`. Do not hand-edit generated `webapp/stories.js` / `StoryLibrary.cs`. Intro decor emoji live in `DECOR_SYMBOLS` inside `webapp/index.html` (manual).
10. **Do not reinstall** Android SDK / Gradle / JDK for this repo. Reuse the host toolchain from sibling **I_Tuoi_Versetti** (see § Android environment below and KB §1).

## Android environment

Riferimento: progetto **I_Tuoi_Versetti** nello stesso workspace Cursor / host Windows (`D:\I_Tuoi_Versetti`).  
**Non** reinstallare Android SDK, cmdline-tools, né una Gradle distribution da zero.

Path canonici verificati (allineati a Versetti `local.properties` + script `tools/*.ps1`):

| Voce | Path |
|------|------|
| Progetto riferimento | `D:\I_Tuoi_Versetti` |
| `sdk.dir` / `ANDROID_HOME` / `ANDROID_SDK_ROOT` | `D:\Android\Sdk` |
| `JAVA_HOME` | `D:\JDK_17` (Oracle 17; default negli script Versetti) |
| Wrapper da copiare | `gradlew`, `gradlew.bat`, `gradle\wrapper\` (Gradle **8.7**) |
| AVD smoke | `Pixel_2_API_30` |
| AGP JW Quiz | `android/build.gradle` = **8.5.2** (Versetti usa 8.6.0; non alzare senza OK umano) |

Gap tipico di `Jw_Quiz_Development/android/`: manca solo wrapper + `local.properties` (SDK già sul disco).

Copiare / impostare (Fase D-CLI, solo dopo OK umano se non già fatto):

```powershell
# JAVA_HOME + SDK in sessione PowerShell
$env:JAVA_HOME = "D:\JDK_17"
$env:ANDROID_HOME = "D:\Android\Sdk"
$env:ANDROID_SDK_ROOT = "D:\Android\Sdk"
$env:PATH = "$env:JAVA_HOME\bin;$env:ANDROID_HOME\platform-tools;$env:PATH"

# sdk.dir → android/local.properties (gitignored; stesso valore di Versetti)
Set-Content -Path "android\local.properties" -Encoding ascii -Value "sdk.dir=D:\\Android\\Sdk"

# gradle wrapper → android/ (da I_Tuoi_Versetti, non regenerate da internet)
Copy-Item "D:\I_Tuoi_Versetti\gradlew","D:\I_Tuoi_Versetti\gradlew.bat" "android\"
New-Item -ItemType Directory -Force -Path "android\gradle" | Out-Null
Copy-Item -Recurse -Force "D:\I_Tuoi_Versetti\gradle\wrapper" "android\gradle\wrapper"
```

Poi dal **root** repo: `python tools/sync_all.py` → `cd android` → `.\gradlew.bat assembleDebug --no-daemon`.  
Dettaglio host / troubles: `.github/KB.md` §1 (Android toolchain) e §13.

## Where to change what

| Intent | Files |
|--------|--------|
| New / edit episode content | `data/episodes.json` → validate → `sync_all` → `verify_episode_parity` |
| Intro decor emoji (immersive) | `DECOR_SYMBOLS` in `webapp/index.html` |
| New PNG concept | `tools/photo_concepts.py` → generate master → `python tools/sync_all.py` |
| Player UX / 3D / modes | `webapp/index.html` only |
| Editor / Cloudflare API | `webapp/classic.html`, `webapp/app.js` (OK + limited scope), `functions/api/*` |
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
- Expanding edits in `webapp/app.js` beyond an explicitly approved rebus-flat scope
- Installing / downloading a fresh Android SDK, JDK, or Gradle instead of reusing `D:\I_Tuoi_Versetti` + `D:\Android\Sdk` + `D:\JDK_17`

Everything else: implement, build, update KB, commit.

## Validation

From **repo root**:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" Jw_Quiz_Development.csproj /p:Configuration=Debug /nologo /verbosity:quiet
python tools/sync_all.py
python tools/verify_episode_parity.py
```

Desktop exit code must be 0 before commit if C# changed. After `webapp/` player changes, `python tools/sync_all.py` + parity is enough (no MSBuild unless C# touched). Preview: prefer MIME-correct server on `:8081` or `npx serve` (plain `http.server` may break ES modules). Deploy: `npx wrangler pages deploy webapp --project-name=jwquiz` with a clean tree.

Android APK (dopo setup § Android environment):

```powershell
$env:JAVA_HOME = "D:\JDK_17"
$env:ANDROID_HOME = "D:\Android\Sdk"
python tools/sync_all.py
cd android
.\gradlew.bat assembleDebug --no-daemon
```

## Do not commit

- `android/.gradle`, `android/build`, `android/app/build`, `android/app/src/main/assets/www`
- `android/local.properties` (path macchina; gitignored)
- `tools/photo_masters/*.png` (regenerable; applied copies live in `Resources/` and `webapp/assets/`)
- `bin/`, `obj/`, `.wrangler/`
- `UserProgress.dat`, `UserStories.dat` (local runtime; gitignored)
