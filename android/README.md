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

- `sdk.dir=D:\\Android\\Sdk` → `android/local.properties` (gitignored)
- Copy `gradlew` / `gradlew.bat` / `gradle/wrapper/` from `D:\I_Tuoi_Versetti` (Gradle 8.7)
- Session: `JAVA_HOME=D:\JDK_17` (not Java 21 on PATH)
- `android/gradle.properties` must set `android.useAndroidX=true` (required by appcompat)

Android Studio: open `android/` after sync + `local.properties` / wrapper present.

CLI:

```powershell
$env:JAVA_HOME = "D:\JDK_17"
$env:ANDROID_HOME = "D:\Android\Sdk"
cd android
.\gradlew.bat assembleDebug --no-daemon
```

APK: `android/app/build/outputs/apk/debug/app-debug.apk`  
Package/activity: `com.jwquiz.app` / `com.jwquiz.app.MainActivity`  
AVD smoke: `Pixel_2_API_30` under `D:\Android\Sdk`.

```powershell
adb install -r app\build\outputs\apk\debug\app-debug.apk
adb shell am start -n com.jwquiz.app/com.jwquiz.app.MainActivity
adb logcat -d | Select-String "chromium|console|jwquiz|WebView"
```

Known build note (resolved 2026-09-29): kotlin-stdlib constraints in `app/build.gradle`.  
Smoke Ep 10 (2026-09-29, Motorola edge 40 `ZY22HFWMGV`, adb-only): reveal/hide + hint toggle OK; stelle policy A (peak ★1 invariato su Nascondi). Screenshot in `.local/smoke/` (gitignored).

## Notes

- Offline-first: `file:///android_asset/www/index.html`
- CDN Three.js still needs network on first load of WebGL mode; CSS fallback works offline
- Same photorealistic assets as desktop (`Resources/`) and web (`webapp/assets/`)
