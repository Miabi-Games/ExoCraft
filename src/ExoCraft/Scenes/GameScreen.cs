using ExoCraft.Globals;

using Godot;

namespace ExoCraft.Scenes;

public partial class GameScreen : Control
{
    [Export]
    private PackedScene? GameMenuOverlay { get; set; }

    public override void _Ready()
    {
        _overlayContainer = GetNode<Control>("%OverlayContainer");
    }

    public override void _UnhandledInput(InputEvent ev)
    {
        if (!GetShouldAcceptInput()) return;

        if (ev.IsActionPressed("ui_cancel"))
        {
            LoadGameMenuScreen();
            GetViewport().SetInputAsHandled();
        }
    }

    private void LoadGameMenuScreen()
    {
        if (GameMenuOverlay is null)
        {
            GD.PushError("GameScreen: GameMenuOverlay is not set.");
            return;
        }

        var menu = GameMenuOverlay.Instantiate<GameMenuOverlay>();

        if (menu is null)
        {
            GD.PushError($"GameScreen: Failed to load GameMenuOverlay.");
            return;
        }

        _overlayContainer.AddChild(menu);
    }

    private static bool GetShouldAcceptInput()
    {
        return InteractiveOverlayTracker.Instance.Count == 0;
    }

    private static Control _overlayContainer = null!;
}
