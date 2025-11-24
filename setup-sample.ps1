#!/usr/bin/env pwsh
# Quick Setup Script for FluentListView Sample (PowerShell)
# This script automates the setup process described in QUICKSTART.md

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "FluentListView Sample - Quick Setup" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if we're in the correct directory
if (-not (Test-Path "FluentListView/FluentListView.sln")) {
    Write-Host "Error: Please run this script from the repository root directory" -ForegroundColor Red
    Write-Host "Current directory: $PWD" -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "Step 1: Building FluentListView library..." -ForegroundColor Yellow
Write-Host ""
Push-Location FluentListView
dotnet build FluentListView.sln -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "Error: Failed to build FluentListView" -ForegroundColor Red
    Pop-Location
    Read-Host "Press Enter to exit"
    exit 1
}
Pop-Location

Write-Host ""
Write-Host "Step 2: Creating sample project..." -ForegroundColor Yellow
Write-Host ""
if (Test-Path "TaskManagerSample") {
    Write-Host "TaskManagerSample directory already exists. Removing..." -ForegroundColor Yellow
    Remove-Item -Path "TaskManagerSample" -Recurse -Force
}
dotnet new winforms -n TaskManagerSample
if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "Error: Failed to create WinForms project" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""
Write-Host "Step 3: Adding FluentListView reference..." -ForegroundColor Yellow
Write-Host ""
Push-Location TaskManagerSample
dotnet add reference ../FluentListView/bin/Release/net9.0-windows/FluentListView.dll
if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "Error: Failed to add reference" -ForegroundColor Red
    Pop-Location
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""
Write-Host "Step 4: Copying sample file..." -ForegroundColor Yellow
Write-Host ""
Copy-Item ../SampleFluentListViewForm.cs . -Force
if (-not $?) {
    Write-Host ""
    Write-Host "Error: Failed to copy sample file" -ForegroundColor Red
    Pop-Location
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""
Write-Host "Step 5: Updating Program.cs..." -ForegroundColor Yellow
Write-Host ""
$programContent = @"
using System;
using System.Windows.Forms;
using FluentListViewSample;

namespace TaskManagerSample
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SampleFluentListViewForm());
        }
    }
}
"@
$programContent | Out-File -FilePath "Program.cs" -Encoding UTF8

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Setup Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "The sample project has been created in: TaskManagerSample" -ForegroundColor Cyan
Write-Host ""
Write-Host "To run the sample, execute:" -ForegroundColor Yellow
Write-Host "  cd TaskManagerSample" -ForegroundColor White
Write-Host "  dotnet run" -ForegroundColor White
Write-Host ""
Write-Host "Or open TaskManagerSample.csproj in Visual Studio" -ForegroundColor Yellow
Write-Host ""

$runNow = Read-Host "Do you want to run the sample now? (Y/N)"
if ($runNow -eq "Y" -or $runNow -eq "y") {
    Write-Host ""
    Write-Host "Running sample..." -ForegroundColor Yellow
    Write-Host ""
    dotnet run
}

Pop-Location
Write-Host ""
Write-Host "Press Enter to exit..." -ForegroundColor Gray
Read-Host
