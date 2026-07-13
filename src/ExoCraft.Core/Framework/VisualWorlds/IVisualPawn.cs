using ExoCraft.Framework.Math;

namespace ExoCraft.Framework.VisualWorlds;

public interface IVisualPawn
{
    void Free();
    void SyncPosition(double3xform position);
}
