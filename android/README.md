# JW Quiz — Android

WebView shell that packages the immersive web experience (Quiz / Rebus 3D / Avventura).

## Sync web assets into the APK

From **repo root** (not from `android/`):

```powershell
python tools/sync_all.py
```

That is the canonical pipeline (photo apply if masters exist + copy `webapp/` → `android/app/src/main/assets/www/`). Equivalent www-only step: `python tools/sync_android_www.py`.

The `www/` folder is **gitignored** (generated). Sync restores `www/.gitkeep` so git stays clean. Always sync before building the APK.

## Build

**Do not install Android SDK / Gradle from scratch.** Reuse host paths from sibling `D:\I_Tuoi_Versetti` — full checklist in `docs/AGENTS.md` § Android environment and `.github/KB.md` §1:

- `sdk.dir=D:\\Android\\Sdk` → `android/local.properties`
- Copy `gradlew` / `gradlew.bat` / `gradle/wrapper/` from `D:\I_Tuoi_Versetti`
- Session: `JAVA_HOME=D:\JDK_17`

Android Studio: open `android/` after sync + `local.properties` / wrapper present.

CLI:

```powershell
$env:JAVA_HOME = "D:\JDK_17"
$env:ANDROID_HOME = "D:\Android\Sdk"
cd android
.\gradlew.bat assembleDebug --no-daemon
```

AVD smoke: `Pixel_2_API_30` under `D:\Android\Sdk`.

## Notes

- Offline-first: `file:///android_asset/www/index.html`
- CDN Three.js still needs network on first load of WebGL mode; CSS fallback works offline
- Same photorealistic assets as desktop (`Resources/`) and web (`webapp/assets/`)
