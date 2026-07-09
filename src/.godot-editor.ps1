param(
  [Parameter(ValueFromRemainingArguments = $true)]
  [string[]] $ExtraArgs
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot

# -- Load config
$configPath = Join-Path $root '.godot-editor.json'
$cfg = if (Test-Path $configPath) {
  try { Get-Content -Raw -Path $configPath | ConvertFrom-Json } catch {
    throw "[Open in Godot] Invalid JSON in $configPath. $($_.Exception.Message)"
  }
} else { [pscustomobject]@{} }

# Required settings (env var name + relative project dir)
$envVarName   = if ($cfg.PSObject.Properties.Name -contains 'envVar'   -and $cfg.envVar)   { $cfg.envVar }   else { 'GODOT_EXE' }
$relProjectDir= if ($cfg.PSObject.Properties.Name -contains 'projectDir' -and $cfg.projectDir){ $cfg.projectDir } else { $null }

# -- Resolve editor exe path (PS 5.1 & 7 compatible)
$godot = [Environment]::GetEnvironmentVariable($envVarName, 'Process')
if ([string]::IsNullOrWhiteSpace($godot)) { $godot = [Environment]::GetEnvironmentVariable($envVarName, 'User') }
if ([string]::IsNullOrWhiteSpace($godot)) { $godot = [Environment]::GetEnvironmentVariable($envVarName, 'Machine') }
if ($godot) { $godot = $godot.Trim().Trim('"', "'") }

if ([string]::IsNullOrWhiteSpace($godot)) { throw "[Open in Godot] Environment variable '$envVarName' is not set (expected full path to Godot editor exe)." }
if (-not (Test-Path -LiteralPath $godot)) { throw "[Open in Godot] Godot editor not found at: $godot (from %$envVarName%)." }

# -- Resolve project directory
if ($relProjectDir) {
  $projDir = Join-Path $root $relProjectDir
  if (-not (Test-Path (Join-Path $projDir 'project.godot'))) {
    throw "[Open in Godot] project.godot not found in configured projectDir: $projDir"
  }
} else {
  $proj = Get-ChildItem -Path $root -Recurse -Filter 'project.godot' -File -ErrorAction SilentlyContinue | Select-Object -First 1
  if (-not $proj) { throw "[Open in Godot] Could not locate project.godot under solution root. Add projectDir to .godot-editor.json." }
  $projDir = $proj.DirectoryName
}

# -- Create the argument list

$argList = @('--path', $projDir)
if ($ExtraArgs) { $argList += $ExtraArgs }

$escaped = foreach ($a in $argList) {
  if ($a -match '[\s"]') {
    '"' + ($a -replace '"','\"') + '"'
  } else {
    $a
  }
}

# -- Launch editor
Start-Process -FilePath $godot -ArgumentList $escaped -WorkingDirectory $projDir
