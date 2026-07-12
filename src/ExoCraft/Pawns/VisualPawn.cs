using ExoCraft.Framework.VisualWorlds;

using Godot;

namespace ExoCraft.Pawns;

public partial class VisualPawn : Node3D, IVisualPawn
{
    void IVisualPawn.Free() => QueueFree();
}
