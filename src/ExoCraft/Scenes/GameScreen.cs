using ExoCraft.Globals;

using Godot;

namespace ExoCraft.Scenes;

public partial class GameScreen : Control
{
    public override void _Ready()
    {
        _overlayContainer = GetNode<Control>("%OverlayContainer");
        _gameMenuOverlayScene = ScreenHelper.LoadPackedScene("Game Screen", ScreenHelper.GameMenuOverlay);
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

    private static void LoadGameMenuScreen()
    {
        ScreenHelper.LoadOverlay(
            "Game Screen",
            _overlayContainer,
            ScreenHelper.GameMenuOverlay,
            _gameMenuOverlayScene!);
    }

    private static bool GetShouldAcceptInput()
    {
        return InteractiveOverlayTracker.Instance.Count == 0;
    }

    private static Control _overlayContainer = null!;
    private static PackedScene? _gameMenuOverlayScene = null;
}
