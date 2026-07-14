using ExoCraft.Framework.Math;

namespace ExoCraft.Framework.VisualWorlds;

public interface IVisualVoxelChunk
{
    void Free();
    void SyncPosition(double3xform position);
}
