using Godot;

namespace ExoCraft.Framework.ScreenLayers;

[GlobalClass]
public partial class ScreenOverlayLayer : ScreenLayer
{
    public sealed override ScreenLayerType ScreenLayerType => ScreenLayerType.Overlay;
}
