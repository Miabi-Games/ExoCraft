using ExoCraft.Framework.VisualWorld;

using Godot;

namespace ExoCraft.Pawns;

public partial class VisualPawn : Node3D, IVisualPawn
{
    void IVisualPawn.Free() => QueueFree();
}
