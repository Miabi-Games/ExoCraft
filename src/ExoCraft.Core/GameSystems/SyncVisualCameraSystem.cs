using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;

namespace ExoCraft.GameSystems;

public class SyncVisualCameraSystem : GameSystem
{
    public SyncVisualCameraSystem(
        ISimWorld simWorld,
        IVisualWorld visualWorld)
    {
        _ecsWorld = simWorld.EcsWorld;
        _visualWorld = visualWorld;
    }

    public override void Initialize()
    {
        _cameras = _ecsWorld.GetEntities()
            .With<Camera>()
            .AsSet();
    }

    public override void Shutdown()
    {
        _cameras.Dispose();
    }

    public override void Update(double delta)
    {
        foreach (var entity in _cameras.GetEntities())
        {
            _visualWorld.CameraTransform = entity.Get<Camera>().Transform;
        }
    }

    private readonly World _ecsWorld;
    private readonly IVisualWorld _visualWorld;

    private EntitySet _cameras = null!;
}
