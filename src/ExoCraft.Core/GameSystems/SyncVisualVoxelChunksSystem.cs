using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;

namespace ExoCraft.GameSystems;

public class SyncVisualVoxelChunksSystem : GameSystem
{
    public SyncVisualVoxelChunksSystem(
        ISimWorld simWorld,
        IVisualWorld visualWorld)
    {
        _ecsWorld = simWorld.EcsWorld;
        _visualWorld = visualWorld;
    }

    public override void Initialize()
    {
        _chunks = _ecsWorld.GetEntities()
            .With<VoxelChunk>()
            .AsSet();
    }

    public override void Shutdown()
    {
        _chunks.Dispose();
    }

    public override void Update(double delta)
    {
        foreach (Entity entity in _chunks.GetEntities())
        {
            var chunk = entity.Get<VoxelChunk>();

            if (chunk.VisualVoxelChunk is { } visualChunk)
            {
                _visualWorld.SyncVoxelChunk(visualChunk, chunk.Transform);
            }
        }
    }

    private readonly World _ecsWorld;
    private readonly IVisualWorld _visualWorld;

    private EntitySet _chunks = null!;
}
