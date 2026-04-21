@echo off
echo Building JW Quiz Development...
msbuild Jw_Quiz_Development.csproj /p:Configuration=Release /p:Platform="Any CPU"
if %errorlevel% neq 0 (
    echo Build failed!
    pause
    exit /b 1
)
echo Build successful! EXE located at bin\Release\Jw_Quiz_Development.exe
pause