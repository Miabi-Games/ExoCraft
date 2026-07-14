# Testing

ExoCraft has two automated test layers.

- `ExoCraft.Core.Tests` uses NUnit to test engine-independent simulation logic.
- Godot integration tests use GdUnit4Net to test scenes and engine-dependent
  behavior inside a headless Godot process.

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

## Run Core Tests Only

Core tests do not require Godot:

```powershell
dotnet test src/ExoCraft.Core.Tests/ExoCraft.Core.Tests.csproj
```

## Test Scope

Godot integration tests are intended for observable engine behavior such as:

- scene and node wiring;
- input dispatch;
- node lifecycle callbacks;
- screen and overlay behavior; and
- synchronization between Godot adapters and the core simulation.

Keep simulation rules in `ExoCraft.Core` and cover them with NUnit tests. Do
not use scene tests as a replacement for focused core tests or as screenshot
tests for visual presentation.
