# AGENTS.md

## Project Overview

ExoCraft is an early-stage voxel automation survival game built with Godot and
C#. The project emphasizes survival as infrastructure, automation with purpose,
and the progression from shelter to a livable home.

Treat the repository as both a game prototype and a public example of clean,
maintainable game architecture.

## Repository Structure

- `src/ExoCraft` contains Godot scenes, nodes, UI, visual presentation, and
  other engine-dependent code.
- `src/ExoCraft.Core` contains engine-independent simulation logic, entity
  components, game systems, interfaces, and math types.
- `src/ExoCraft.Core.Tests` contains automated tests for `ExoCraft.Core`.
- `docs` contains design and technical documentation.

## Architecture Guidelines

- Keep gameplay and simulation logic in `ExoCraft.Core` whenever it does not
  require Godot.
- Keep Godot-specific behavior and presentation in `ExoCraft`.
- Use interfaces to connect the core simulation to visual or engine-specific
  implementations.
- Do not introduce Godot dependencies into `ExoCraft.Core`.
- Treat the simulation world as authoritative. Visual nodes should represent
  simulation state rather than become an independent source of gameplay state.
- Implement gameplay behavior through focused game systems and entity
  components.
- Keep entity components small and primarily data-oriented.
- Preserve explicit lifecycle stages such as creation, initialization,
  updating, rendering, shutdown, and disposal.
- Prefer incremental extensions to the existing architecture over broad
  rewrites.

## Godot Guidelines

- Use C# for project logic unless a task specifically requires another
  approach.
- Keep `.tscn` and `.tres` changes focused and reviewable.
- Do not manually edit generated `.uid` or `.import` files unless the task
  specifically requires it and the format is understood.
- Preserve exported property names and scene node paths unless all affected
  scenes and scripts are updated together.
- Be careful when renaming scenes, scripts, nodes, or resources because Godot
  references may depend on their paths or unique names.
- Avoid placing simulation rules directly in scene scripts when they can live
  in the core project.
- Follow the existing screen-layer and game-system lifecycle instead of
  bypassing it.

## C# Style

Follow `.editorconfig` and the conventions already present in nearby code.

- Use four spaces in C# files and two spaces in project, JSON, YAML, and
  Markdown files.
- Use file-scoped namespaces.
- Keep nullable reference types enabled and avoid suppressing nullable warnings
  without a clear reason.
- Keep implicit usings disabled.
- Prefer explicit, readable code over clever or highly compressed code.
- Use descriptive names that reflect domain concepts.
- Keep methods focused and classes reasonably small.
- Follow the formatting and member-ordering style of the file being edited.
- Aim for 80-column readability where practical; treat 120 columns as the upper
  limit.
- Add comments when they explain intent, constraints, or non-obvious behavior.
  Do not narrate straightforward code.
- Do not introduce primary constructors unless the project convention changes.
- Avoid unrelated formatting changes.

## Testing

- Add or update tests when changing engine-independent behavior.
- Place core unit tests under `src/ExoCraft.Core.Tests/UnitTests` in the
  corresponding namespace directory.
- Follow existing NUnit test conventions and descriptive test names.
- Test observable behavior and lifecycle effects rather than private
  implementation details.
- Include cleanup and disposal behavior in tests when resources or ECS entities
  are involved.
- Use mocks at engine boundaries where appropriate.
- Run the most focused relevant tests first, followed by the full test project
  when practical.

Useful validation commands:

```powershell
dotnet test src/ExoCraft.Core.Tests/ExoCraft.Core.Tests.csproj
dotnet build src/ExoCraft.sln
```

If a command cannot run because the required Godot or .NET version is
unavailable, report that limitation clearly.

## Working Practices

- Inspect relevant code, scenes, tests, and documentation before making
  changes.
- Check for existing uncommitted work before editing.
- Preserve user changes and do not revert, overwrite, or reformat unrelated
  work.
- Keep changes limited to the requested scope.
- Update documentation when a change alters documented behavior, architecture,
  or controls.
- Do not add dependencies unless they materially simplify the requested work
  and fit the architecture.
- Do not create commits, branches, or pull requests unless explicitly
  requested.
- Do not modify licensing or source-availability language without explicit
  direction.
- Never use destructive Git operations unless explicitly authorized.

## Commit Messages

When explicitly asked to create a commit, follow the convention established by
the repository history:

- Write the summary in lowercase imperative form, such as `add`, `create`,
  `implement`, `update`, `refactor`, or `remove`.
- Describe the outcome directly without a Conventional Commit prefix such as
  `feat:` or `fix:`.
- Do not end the summary with a period.
- Keep the summary concise but specific enough to identify the change.
- Put filenames in backticks when referring to them in the message.
- Add a body only when useful to explain context, rationale, or tradeoffs.
- Use direct milestone messages such as `start version 0.2.0` or `tag v0.1.0`
  for version-related commits.

## Verification and Handoff

Before declaring implementation work complete:

1. Review the final diff for accidental or unrelated changes.
2. Build the affected project when practical.
3. Run relevant automated tests.
4. Check Godot scene and resource references when they were changed.
5. Report what changed, what was verified, and any remaining limitations.

Do not claim a build, test, or runtime check passed unless it was actually run
successfully.
