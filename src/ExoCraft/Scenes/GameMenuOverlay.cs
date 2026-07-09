using ExoCraft.Globals;

using Godot;

namespace ExoCraft.Scenes;

public partial class GameMenuOverlay : Panel
{
    public override void _Ready()
    {
        InteractiveOverlayTracker.Instance.TrackUntilExit(this);
        var menu = GetNode<MainMenu>("MainMenu");
        menu.ExitMenu += () => Visible = false;
    }
}
