#!/bin/bash

# Build script for WinForms TreeListView Demo Sample
# This script builds the sample project and verifies it compiles successfully

set -e  # Exit on error

echo "=========================================="
echo "Building FluentListView Sample"
echo "=========================================="
echo ""

# Build the entire solution
echo "Building FluentListView.sln..."
dotnet build FluentListView.sln -c Release /p:EnableWindowsTargeting=true

echo ""
echo "=========================================="
echo "Build Completed Successfully!"
echo "=========================================="
echo ""

# Check if the sample was built
SAMPLE_DLL="Samples/WinFormsTreeListViewDemo/bin/Release/net9.0-windows/WinFormsTreeListViewDemo.dll"
if [ -f "$SAMPLE_DLL" ]; then
    echo "✓ Sample project built successfully: $SAMPLE_DLL"
else
    echo "✗ Sample project DLL not found: $SAMPLE_DLL"
    exit 1
fi

echo ""
echo "To run the sample (requires Windows environment):"
echo "  dotnet run --project Samples/WinFormsTreeListViewDemo/WinFormsTreeListViewDemo.csproj"
echo ""
