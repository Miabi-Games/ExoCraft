using ExoCraft.Framework.Math;
using ExoCraft.Framework.VisualWorlds;

namespace ExoCraft.EntityComponents;

public struct VoxelChunk
{
    public IVisualVoxelChunk? VisualVoxelChunk;
    public double3xform Transform;
}
