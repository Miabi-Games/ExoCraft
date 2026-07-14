using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.Math;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;

namespace ExoCraft.GameSystems;

public class VoxelChunkLifecycleSystem : GameSystem
{
    public VoxelChunkLifecycleSystem(
        ISimWorld simWorld,
        IVisualWorld visualWorld)
    {
        _ecsWorld = simWorld.EcsWorld;
        _visualWorld = visualWorld;
    }

    public override void Initialize()
    {
        for (int z = 0; z < ChunkCountPerAxis; z++)
        {
            for (int x = 0; x < ChunkCountPerAxis; x++)
            {
                CreateChunk(x, z);
            }
        }
    }

    public override void Shutdown()
    {
        foreach (Entity entity in _entities)
        {
            var chunk = entity.Get<VoxelChunk>();

            chunk.VisualVoxelChunk?.Free();
            entity.Dispose();
        }
    }

    private void CreateChunk(int x, int z)
    {
        double arrayOffset = (ChunkCountPerAxis - 1) * ChunkSize / 2.0;
        var transform = new double3xform(
            (x * ChunkSize - arrayOffset, ChunkCenterY, z * ChunkSize - arrayOffset),
            double3basis.identity,
            1.0);
        var chunk = new VoxelChunk
        {
            VisualVoxelChunk = _visualWorld.CreateVoxelChunk(),
            Transform = transform,
        };
        Entity entity = _ecsWorld.CreateEntity();

        entity.Set(chunk);
        entity.Set<Dirty>();
        _entities[z * ChunkCountPerAxis + x] = entity;
    }

    private const int ChunkCountPerAxis = 4;
    private const double ChunkSize = 8.0;
    private const double ChunkCenterY = -4.0;

    private readonly World _ecsWorld;
    private readonly IVisualWorld _visualWorld;
    private readonly Entity[] _entities =
        new Entity[ChunkCountPerAxis * ChunkCountPerAxis];
}
