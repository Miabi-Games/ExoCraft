using ExoCraft.Framework.Math;
using ExoCraft.Framework.VisualWorlds;

namespace ExoCraft.EntityComponents;

public struct Pawn
{
    public IVisualPawn? VisualPawn;
    public double3xform Transform;
}
