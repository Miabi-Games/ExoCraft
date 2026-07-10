using ExoCraft.Globals;

using Godot;

namespace ExoCraft.Scenes;

public partial class GameScreen : Control
{
    public override void _Ready()
    {
        _gameMenu = GetNode<GameMenuOverlay>("%GameMenu");
    }

    public override void _UnhandledInput(InputEvent ev)
    {
        if (!GetShouldAcceptInput()) return;

        if (ev.IsActionPressed("ui_cancel"))
        {
            _gameMenu.Visible = true;
        }

        GetViewport().SetInputAsHandled();
    }

    private static bool GetShouldAcceptInput()
    {
        return InteractiveOverlayTracker.Instance.Count == 0;
    }

    GameMenuOverlay _gameMenu = null!;
}
