using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.Math;
using ExoCraft.Framework.SimWorlds;

namespace ExoCraft.GameSystems;

public class CameraLifecycleSystem : GameSystem
{
    public CameraLifecycleSystem(ISimWorld simWorld)
    {
        _ecsWorld = simWorld.EcsWorld;
    }

    public override void Initialize()
    {
        var rotation = double3basis.identity;
        rotation.rotate_local_x(-System.Math.PI / 6.0);

        var camera = new Camera
        {
            Transform = new(
                position: (0.0, 6.0, 13.0),
                rotation,
                scale: 1.0),
        };

        _entity = _ecsWorld.CreateEntity();
        _entity.Set(camera);
    }

    public override void Shutdown()
    {
        _entity.Dispose();
    }

    private readonly World _ecsWorld;

    private Entity _entity;
}
