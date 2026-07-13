using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.SimWorlds;

namespace ExoCraft.GameSystems;

public class SyncVisualPawnsSystem : GameSystem
{
    public SyncVisualPawnsSystem(ISimWorld simWorld)
    {
        _ecsWorld = simWorld.EcsWorld;
    }

    public override void Initialize()
    {
        _pawns = _ecsWorld.GetEntities()
            .With<Pawn>()
            .AsSet();
    }

    public override void Shutdown()
    {
        _pawns.Dispose();
    }

    public override void Update(double delta)
    {
        foreach (var entity in _pawns.GetEntities())
        {
            var pawn = entity.Get<Pawn>();
            pawn.VisualPawn?.SyncPosition(pawn.Position);
        }
    }

    private readonly World _ecsWorld;

    private EntitySet _pawns = null!;
}
