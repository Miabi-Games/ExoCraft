using Godot;

namespace ExoCraft.Scenes;

public partial class BootScreen : Panel
{
    [Export]
    private double TimeoutSeconds { get; set; } = 3.0;

    [Export]
    private PackedScene? NextScene { get; set; }

    public override async void _Ready()
    {
        if (NextScene is null)
        {
            GD.PushError("BootScreen: NextScene is not set.");
            return;
        }

        await ToSignal(GetTree().CreateTimer(TimeoutSeconds), SceneTreeTimer.SignalName.Timeout);

        Error result = GetTree().ChangeSceneToPacked(NextScene);

        if (result != Error.Ok)
        {
            GD.PushError($"BootScreen: Failed to change scene. Error: {result}");
        }
    }
}
