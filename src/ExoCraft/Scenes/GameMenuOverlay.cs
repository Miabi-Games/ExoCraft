using ExoCraft.Framework.ScreenLayers;

namespace ExoCraft.Scenes;

public partial class GameMenuOverlay : ScreenOverlayLayer
{
    public override void _Ready()
    {
        var menu = GetNode<MainMenu>("MainMenu");
        menu.ExitMenu += () => ScreenLayerManager.PopScreenOverlay("MainMenuOverlay", this);
    }
}
