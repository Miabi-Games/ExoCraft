using ExoCraft.Framework.Math;

namespace ExoCraft.Framework.VisualWorlds;

public interface IVisualWorld
{
    double3xform CameraTransform { get; set; }

    IVisualPawn CreatePlayerPawn();
    void SyncPawn(IVisualPawn visualPawn, double3xform transform);
}
