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

        var pawn = new Pawn
        {
            VisualPawn = visualPawn,
        };

        _entity = _ecsWorld.CreateEntity();

        _entity.Set<PlayerPawn>();
        _entity.Set<Pawn>(pawn);
    }

    public override void Shutdown()
    {
        var pawn = _entity.Get<Pawn>();

        pawn.VisualPawn?.Free();
        _entity.Dispose();
    }

    private readonly World _ecsWorld;
    private readonly IVisualWorld _visualWorld;

    private Entity _entity;
}
