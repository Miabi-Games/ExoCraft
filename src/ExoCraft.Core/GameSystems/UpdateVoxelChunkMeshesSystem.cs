using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.Math;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;

using System.Collections.Generic;

namespace ExoCraft.GameSystems;

public class UpdateVoxelChunkMeshesSystem : GameSystem
{
    public UpdateVoxelChunkMeshesSystem(ISimWorld simWorld)
    {
        _ecsWorld = simWorld.EcsWorld;
    }

    public override void Initialize()
    {
        _dirtyChunks = _ecsWorld.GetEntities()
            .With<VoxelChunk>()
            .With<Dirty>()
            .AsSet();
    }

    public override void Shutdown()
    {
        _dirtyChunks.Dispose();
    }

    public override void Update(double delta)
    {
        foreach (Entity entity in _dirtyChunks.GetEntities())
        {
            VoxelChunk chunk = entity.Get<VoxelChunk>();

            chunk.VisualVoxelChunk?.SetMesh(Vertices, Indices);
            entity.Remove<Dirty>();
        }
    }

    private static IReadOnlyList<VoxelVertex> CreateVertices()
    {
        const float min = -4.0f;
        const float max = 4.0f;
        var vertices = new List<VoxelVertex>(24);

        AddFace(vertices, float3.forward,
            (min, min, min), (min, max, min),
            (max, max, min), (max, min, min));
        AddFace(vertices, float3.back,
            (min, min, max), (max, min, max),
            (max, max, max), (min, max, max));
        AddFace(vertices, float3.left,
            (min, min, max), (min, max, max),
            (min, max, min), (min, min, min));
        AddFace(vertices, float3.right,
            (max, min, min), (max, max, min),
            (max, max, max), (max, min, max));
        AddFace(vertices, float3.up,
            (min, max, min), (min, max, max),
            (max, max, max), (max, max, min));
        AddFace(vertices, float3.down,
            (min, min, max), (min, min, min),
            (max, min, min), (max, min, max));

        return vertices;
    }

    private static IReadOnlyList<int> CreateIndices()
    {
        var indices = new List<int>(36);

        for (int face = 0; face < 6; face++)
        {
            int firstVertex = face * 4;

            indices.Add(firstVertex);
            indices.Add(firstVertex + 2);
            indices.Add(firstVertex + 1);
            indices.Add(firstVertex);
            indices.Add(firstVertex + 3);
            indices.Add(firstVertex + 2);
        }

        return indices;
    }

    private static void AddFace(
        ICollection<VoxelVertex> vertices,
        float3 normal,
        float3 bottomLeft,
        float3 topLeft,
        float3 topRight,
        float3 bottomRight)
    {
        vertices.Add(new() { Position = bottomLeft, Normal = normal });
        vertices.Add(new() { Position = topLeft, Normal = normal });
        vertices.Add(new() { Position = topRight, Normal = normal });
        vertices.Add(new() { Position = bottomRight, Normal = normal });
    }

    private static readonly IReadOnlyList<VoxelVertex> Vertices =
        CreateVertices();
    private static readonly IReadOnlyList<int> Indices = CreateIndices();

    private readonly World _ecsWorld;
    private EntitySet _dirtyChunks = null!;
}
