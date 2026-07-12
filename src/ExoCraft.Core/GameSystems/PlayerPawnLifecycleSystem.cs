using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;

namespace ExoCraft.GameSystems;

public class PlayerPawnLifecycleSystem : GameSystem
{
    public PlayerPawnLifecycleSystem(
        ISimWorld simWorld,
        IVisualWorld visualWorld)
    {
        _ecsWorld = simWorld.EcsWorld;
        _visualWorld = visualWorld;
    }

    public override void Initialize()
    {
        var visualPawn = _visualWorld.CreatePlayerPawn();

        _entity = _ecsWorld.CreateEntity();

        _entity.Set<PlayerPawn>(new());
        _entity.Set<IVisualPawn>(visualPawn);
    }

    public override void Shutdown()
    {
        var visualPawn = _entity.Get<IVisualPawn>();

        visualPawn.Free();
        _entity.Dispose();
    }

    private readonly World _ecsWorld;
    private readonly IVisualWorld _visualWorld;

    private Entity _entity;
}
