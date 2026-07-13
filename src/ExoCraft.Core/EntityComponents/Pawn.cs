using ExoCraft.Framework.Math;
using ExoCraft.Framework.VisualWorlds;

namespace ExoCraft.EntityComponents;

public class Pawn
{
    public IVisualPawn? VisualPawn;
    public double3xform Position = double3xform.identity;
}
