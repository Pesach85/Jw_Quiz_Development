# JW Quiz Development

## Project Overview
This is a Windows Forms application developed in C# using .NET Framework 4.7.2. The application appears to be a quiz or educational tool related to Jehovah's Witnesses (JW), consisting of 12 interactive "stories" or sections with revealable elements.

## Architecture
- **Framework**: .NET Framework 4.7.2
- **UI**: Windows Forms
- **Language**: C#
- **Build Tool**: MSBuild (via .csproj)

## Application Structure
- **Form1**: Main menu form with navigation to different stories and screen controls
- **Form2-Form13**: Individual quiz/story forms (12 stories + FINE form)
- **FINE**: End/conclusion form
- **Forms_list**: Utility class managing form navigation and screen size controls

## Key Features
- Fullscreen toggle functionality
- Interactive quiz elements with revealable symbols, hints, and solutions
- Navigation between 12 story sections
- Menu-based navigation system

## Forms Description
- **Form1**: Entry point, menu with story selection
- **Form2-Form13**: Quiz sections with interactive buttons for revealing content
- **FINE**: Final form with navigation back to sections

## Build Instructions
### Prerequisites
- .NET Framework 4.7.2 or compatible
- Visual Studio 2019+ or MSBuild

### Building EXE
1. Open the solution in Visual Studio
2. Build in Release mode
3. EXE will be in `bin\Release\Jw_Quiz_Development.exe`

### Command Line Build
```bash
msbuild Jw_Quiz_Development.csproj /p:Configuration=Release
```

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
- Set up GitHub remote repository
- Push code to GitHub
- Consider migration to .NET Core/.NET 5+ for modern Windows deployment