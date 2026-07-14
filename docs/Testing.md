# Testing

ExoCraft has two automated test layers.

- `ExoCraft.Core.Tests` uses NUnit to test engine-independent simulation logic.
- Godot regression tests use GdUnit4Net to test previously incorrect scene and
  engine-dependent behavior inside a headless Godot process.

## Run All Tests

Set `GODOT_4_7_MONO_EXE_PATH` to the Godot 4.7 .NET executable, then run:

```powershell
.\src\RunTests.ps1
```

The script maps the version-specific variable to the `GODOT_BIN` variable that
GdUnit4Net expects, then runs the solution through the .NET test platform. This
includes both the NUnit and GdUnit4Net tests.

The Godot test packages and test sources are included only in Debug builds.
They are excluded from export configurations.

## Run Tests in Visual Studio

Godot regression tests can be run from Test Explorer. Visual Studio requires a
`GODOT_BIN` environment variable containing the path to the Godot 4.7 .NET
executable. Set it to the same executable referenced by
`GODOT_4_7_MONO_EXE_PATH`.

Restart Visual Studio after adding or changing `GODOT_BIN`. Visual Studio reads
environment variables when it starts, and GdUnit4Net marks tests requiring the
Godot runtime as skipped when that executable is not available to the test
process.

If the solution-wide test settings are not already active, use **Test >
Configure Run Settings > Select Solution Wide runsettings File** and select
`src/.runsettings`.

`RunTests.ps1` does not require `GODOT_BIN` to be set permanently. The script
reads the repository-versioned `GODOT_4_7_MONO_EXE_PATH` variable and maps it to
`GODOT_BIN` for the lifetime of the command-line test process. Test Explorer
does not invoke that script, so Visual Studio must inherit `GODOT_BIN` directly
from the environment.

## Run Core Tests Only

Core tests do not require Godot:

```powershell
dotnet test src/ExoCraft.Core.Tests/ExoCraft.Core.Tests.csproj
```

## Test Scope

Godot regression tests are integration-style tests. The regression label
describes why a test exists: to prevent a previously incorrect behavior from
returning. The integration label describes how it works: by exercising several
Godot components together inside a real scene tree.

These tests are intended for observable engine behavior such as:

- scene and node wiring;
- input dispatch;
- node lifecycle callbacks;
- screen and overlay behavior; and
- synchronization between Godot adapters and the core simulation.

Keep simulation rules in `ExoCraft.Core` and cover them with NUnit tests. Do
not use scene tests as a replacement for focused core tests or as screenshot
tests for visual presentation.
