using ExoCraft.Globals;

using Godot;

namespace ExoCraft.Scenes;

public partial class MainMenuScreen : Panel
{
    public override void _Ready()
    {
        InteractiveOverlayTracker.Instance.TrackUntilExit(this);

    }
}
