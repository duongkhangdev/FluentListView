@echo off
REM Quick Setup Script for FluentListView Sample
REM This script automates the setup process described in QUICKSTART.md

echo ========================================
echo FluentListView Sample - Quick Setup
echo ========================================
echo.

REM Check if we're in the correct directory
if not exist "FluentListView\FluentListView.sln" (
    echo Error: Please run this script from the repository root directory
    echo Current directory: %CD%
    pause
    exit /b 1
)

echo Step 1: Building FluentListView library...
echo.
cd FluentListView
dotnet build FluentListView.sln -c Release
if errorlevel 1 (
    echo.
    echo Error: Failed to build FluentListView
    pause
    exit /b 1
)
cd ..

echo.
echo Step 2: Creating sample project...
echo.
if exist TaskManagerSample (
    echo TaskManagerSample directory already exists. Removing...
    rmdir /s /q TaskManagerSample
)
dotnet new winforms -n TaskManagerSample
if errorlevel 1 (
    echo.
    echo Error: Failed to create WinForms project
    pause
    exit /b 1
)

echo.
echo Step 3: Adding FluentListView reference...
echo.
cd TaskManagerSample
dotnet add reference ..\FluentListView\bin\Release\net9.0-windows\FluentListView.dll
if errorlevel 1 (
    echo.
    echo Error: Failed to add reference
    cd ..
    pause
    exit /b 1
)

echo.
echo Step 4: Copying sample file...
echo.
copy ..\SampleFluentListViewForm.cs . >nul
if errorlevel 1 (
    echo.
    echo Error: Failed to copy sample file
    cd ..
    pause
    exit /b 1
)

echo.
echo Step 5: Updating Program.cs...
echo.
(
echo using System;
echo using System.Windows.Forms;
echo using FluentListViewSample;
echo.
echo namespace TaskManagerSample
echo {
echo     static class Program
echo     {
echo         [STAThread]
echo         static void Main^(^)
echo         {
echo             Application.EnableVisualStyles^(^);
echo             Application.SetCompatibleTextRenderingDefault^(false^);
echo             Application.Run^(new SampleFluentListViewForm^(^)^);
echo         }
echo     }
echo }
) > Program.cs

echo.
echo ========================================
echo Setup Complete!
echo ========================================
echo.
echo The sample project has been created in: TaskManagerSample
echo.
echo To run the sample, execute:
echo   cd TaskManagerSample
echo   dotnet run
echo.
echo Or open TaskManagerSample.csproj in Visual Studio
echo.

set /p runnow="Do you want to run the sample now? (Y/N): "
if /i "%runnow%"=="Y" (
    echo.
    echo Running sample...
    echo.
    dotnet run
)

cd ..
echo.
echo Press any key to exit...
pause >nul
