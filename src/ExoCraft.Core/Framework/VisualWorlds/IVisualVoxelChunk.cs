using ExoCraft.Framework.Math;

using System.Collections.Generic;

namespace ExoCraft.Framework.VisualWorlds;

public interface IVisualVoxelChunk
{
    void Free();
    void SetMesh(
        IReadOnlyList<VoxelVertex> vertices,
        IReadOnlyList<int> indices);
    void SyncPosition(double3xform position);
}
