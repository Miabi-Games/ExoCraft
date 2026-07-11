using ExoCraft.Framework.ScreenLayers;

using Godot;

namespace ExoCraft.Scenes;

public partial class BootScreen : Panel
{
    [Export]
    private double TimeoutSeconds { get; set; } = 3.0;

    public override async void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;

        await ToSignal(GetTree().CreateTimer(TimeoutSeconds), SceneTreeTimer.SignalName.Timeout);

        LoadMainMenu();
    }

    public override void _Input(InputEvent ev)
    {
        if (ev is InputEventMouseButton && ev.IsPressed()) LoadMainMenu();
    }

    public override void _UnhandledInput(InputEvent ev)
    {
        if (ev is InputEventKey && ev.IsPressed()) LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        ScreenLayerManager.SwitchRootScreenLayer("BootScreen", ScreenLayerInfo.MainMenuScreen);
    }
}
