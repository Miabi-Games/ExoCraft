using Godot;

namespace ExoCraft.Framework.ScreenLayers;

[GlobalClass]
public abstract partial class ScreenLayer : Control
{
    public abstract ScreenLayerType ScreenLayerType { get; }
    public virtual ScreenLayerMouseHandling MouseHandling => ScreenLayerMouseHandling.Visible;

    public virtual void EnterScreenLayer() { }
    public virtual void ExitScreenLayer() { }
}
