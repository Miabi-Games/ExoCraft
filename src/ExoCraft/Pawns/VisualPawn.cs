using ExoCraft.Framework.Math;
using ExoCraft.Framework.VisualWorlds;

using Godot;

namespace ExoCraft.Pawns;

public partial class VisualPawn : Node3D, IVisualPawn
{
    void IVisualPawn.Free() => QueueFree();

    void IVisualPawn.SyncPosition(double3xform position)
    {
        var scale = (float)position.scale;
        var basis = new Basis(
            ToVector3(position.rotation.x) * scale,
            ToVector3(position.rotation.y) * scale,
            ToVector3(position.rotation.z) * scale);

        GlobalTransform = new(basis, ToVector3(position.position));
    }

    private static Vector3 ToVector3(double3 value)
        => new((float)value.x, (float)value.y, (float)value.z);
}
