# JW Quiz Development

## Project Overview
This is a Windows Forms quiz application developed in C# on .NET Framework 4.7.2. The app presents Bible-themed rebus episodes with progressive reveal, hint mechanics, XP progression, and a unified data-driven renderer.

## Architecture
- **Framework**: .NET Framework 4.7.2
- **UI**: Windows Forms
- **Language**: C#
- **Build Tool**: MSBuild (via .csproj)

## Application Structure
- **Form1**: Main menu form with navigation to different stories and screen controls
- **DynamicStoryForm**: Unified runtime renderer for all built-in stories (ID 1-18) and user-created stories
- **FINE**: End/conclusion form
- **Forms_list**: Utility class managing form navigation and screen size controls
- **StoryLibrary / StoryEngine / StoryResources**: Data catalog, progression flow, and centralized image loading

## Key Features
- Fullscreen toggle functionality
- Interactive rebus elements with revealable symbols, hints, and solutions
- Navigation between 18 built-in story sections
- Menu-based navigation system
- XP, badges, and stars (3/2/1) based on help usage
- In-app story editor with user story persistence

## Forms Description
- **Form1**: Entry point, menu with story selection
- **DynamicStoryForm**: Data-driven quiz screen with hidden slots, animated hint slot, and progressive reveal
- **FINE**: Final form with navigation back to sections

## Gameplay Enrichment Plan
- Move from duplicated story forms to a single data-driven story engine.
- Anchor each section to a biblical keyword, scripture reference, hint, and solution.
- Add engagement context: spiritual takeaway, character purpose, and loyalty themes.
- Future UI goals: progress tracking, badges, study notes, and replay motivation.

## Build Instructions
### Prerequisites
- .NET Framework 4.7.2 or compatible
- Visual Studio 2019+ or MSBuild
- Targeting Pack .NET Framework 4.7.2

### Building EXE
#### Option 1: Automated Script
Run `build.bat` in the project directory.

#### Option 2: Manual Build
1. Open the solution in Visual Studio
2. Build in Release mode
3. EXE will be in `bin\Release\Jw_Quiz_Development.exe`

#### Option 3: Command Line
```bash
msbuild Jw_Quiz_Development.csproj /p:Configuration=Release
```

### Build validation
Recent validation is executed with Visual Studio 2022 MSBuild and the project compiles successfully in Debug.

### Distribution
- Copy the entire `bin\Release\` folder to target Windows machine
- Ensure .NET Framework 4.7.2 is installed
- Run `Jw_Quiz_Development.exe`

## Dependencies
- System.Windows.Forms
- System.Drawing
- Standard .NET Framework libraries

## Resources
- Extensive emoji and image resources in Resources/ folder
- Localized strings (Italian interface)

## Version Control
- Git repository initialized
- .gitignore configured for build artifacts

## Next Steps
- Configure production environment variables for Cloudflare admin analytics (`ADMIN_SECRET`)
- Review multilingual glossary quality for long Bible references and captions
- Evaluate migration from BinaryFormatter persistence to JSON format

## KB Update 2026-04-22 (Decisioni e Valutazioni)

### Decisioni confermate
- Libreria storie riallineata alla mappatura reale dei form (ID 1-12) e non piu' alla vecchia lista errata.
- Aggiunti 6 episodi dinamici (ID 13-18) con rendering unico tramite DynamicStoryForm.
- Introdotto StoryEditorForm con salvataggio su UserStories.dat per generare nuove storie direttamente dall'app.
- Scoring XP dinamico in base agli aiuti usati (100 base, -20 per reveal/hint, minimo 20).
- Progress panel rinnovato con indicatori livello/badge e statistiche rapide.

### Correzione errore C# Dev Kit
- Il progetto e' stato convertito da formato legacy non-SDK a SDK-style:
	- Sdk: Microsoft.NET.Sdk.WindowsDesktop
	- TargetFramework: net472
	- UseWindowsForms: true
- Questa conversione elimina il warning di progetto non supportato in C# Dev Kit.

### Miglioria UX richiesta oggi
- Il suggerimento nelle storie a rebus dinamiche e' ora animato con effetto pulsante (colore + dimensione emoji), fino alla rivelazione dell'indizio.

### Valutazione tecnica
- Build locale validata con MSBuild (Debug AnyCPU).
- Compatibilita' mantenuta con WinForms .NET Framework 4.7.2.
- Runtime desktop unificato su renderer dinamico; legacy statico dismesso.
