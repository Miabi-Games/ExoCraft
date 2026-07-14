$ErrorActionPreference = "Stop"

$godotPath = [Environment]::GetEnvironmentVariable(
    "GODOT_4_7_MONO_EXE_PATH")

if ([string]::IsNullOrWhiteSpace($godotPath))
{
    throw "GODOT_4_7_MONO_EXE_PATH is not set."
}

if (!(Test-Path -LiteralPath $godotPath -PathType Leaf))
{
    throw "Godot was not found at GODOT_4_7_MONO_EXE_PATH: $godotPath"
}

$env:GODOT_BIN = $godotPath

dotnet test "$PSScriptRoot\ExoCraft.sln" `
    --configuration Debug `
    --settings "$PSScriptRoot\.runsettings"

exit $LASTEXITCODE
