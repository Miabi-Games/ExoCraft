using Godot;

namespace ExoCraft.Framework.ScreenLayers;

[GlobalClass]
public partial class RootScreenLayer : ScreenLayer
{
    public sealed override ScreenLayerType ScreenLayerType => ScreenLayerType.Root;
}
