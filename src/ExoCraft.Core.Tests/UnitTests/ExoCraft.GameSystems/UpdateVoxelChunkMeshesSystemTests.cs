using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.Math;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;
using ExoCraft.GameSystems;
using ExoCraft.Testing;

using Moq;

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace UnitTests.ExoCraft.GameSystems;

[TestFixture, CommitStage, UnitTest]
public class UpdateVoxelChunkMeshesSystemTests
{
    [Test]
    public void Test_001_UpdateMethod_ShouldSetDirtyChunkMeshToEightMeterCube()
    {
        using var fixture = new TestFixture();
        var visualChunk = new Mock<IVisualVoxelChunk>();
        Entity entity = fixture.CreateChunk(visualChunk.Object, isDirty: true);
        IReadOnlyList<VoxelVertex>? meshVertices = null;
        IReadOnlyList<int>? meshIndices = null;
        visualChunk
            .Setup(chunk => chunk.SetMesh(
                It.IsAny<IReadOnlyList<VoxelVertex>>(),
                It.IsAny<IReadOnlyList<int>>()))
            .Callback<IReadOnlyList<VoxelVertex>, IReadOnlyList<int>>(
                (vertices, indices) =>
                {
                    meshVertices = vertices;
                    meshIndices = indices;
                });

        fixture.System.Update(1.0 / 60.0);

        visualChunk.Verify(chunk => chunk.SetMesh(
            It.IsAny<IReadOnlyList<VoxelVertex>>(),
            It.IsAny<IReadOnlyList<int>>()), Times.Once);
        Assert.That(meshVertices, Is.Not.Null);
        Assert.That(meshIndices, Is.Not.Null);
        Assert.That(IsEightMeterCube(meshVertices!), Is.True);
        Assert.That(
            HasSixClockwiseFaces(meshVertices!, meshIndices!),
            Is.True);
        Assert.That(entity.Has<Dirty>(), Is.False);
    }

    [Test]
    public void Test_002_UpdateMethod_ShouldNotSetCleanChunkMesh()
    {
        using var fixture = new TestFixture();
        var visualChunk = new Mock<IVisualVoxelChunk>();

        fixture.CreateChunk(visualChunk.Object, isDirty: false);
        fixture.System.Update(1.0 / 60.0);

        visualChunk.Verify(chunk => chunk.SetMesh(
            It.IsAny<IReadOnlyList<VoxelVertex>>(),
            It.IsAny<IReadOnlyList<int>>()), Times.Never);
    }

    [Test]
    public void Test_003_UpdateMethod_ShouldOnlySetMeshOnceUntilMarkedDirtyAgain()
    {
        using var fixture = new TestFixture();
        var visualChunk = new Mock<IVisualVoxelChunk>();
        Entity entity = fixture.CreateChunk(visualChunk.Object, isDirty: true);

        fixture.System.Update(1.0 / 60.0);
        fixture.System.Update(1.0 / 60.0);
        entity.Set<Dirty>();
        fixture.System.Update(1.0 / 60.0);

        visualChunk.Verify(chunk => chunk.SetMesh(
            It.IsAny<IReadOnlyList<VoxelVertex>>(),
            It.IsAny<IReadOnlyList<int>>()), Times.Exactly(2));
    }

    private static bool IsEightMeterCube(
        IReadOnlyList<VoxelVertex> vertices)
    {
        if (vertices.Count != 24)
        {
            return false;
        }

        foreach (VoxelVertex vertex in vertices)
        {
            if (System.Math.Abs(vertex.Position.x) != 4.0f ||
                System.Math.Abs(vertex.Position.y) != 4.0f ||
                System.Math.Abs(vertex.Position.z) != 4.0f ||
                !IsAxisNormal(vertex.Normal))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsAxisNormal(float3 normal)
    {
        float componentMagnitude =
            System.Math.Abs(normal.x) +
            System.Math.Abs(normal.y) +
            System.Math.Abs(normal.z);

        return componentMagnitude == 1.0f;
    }

    private static bool HasSixClockwiseFaces(
        IReadOnlyList<VoxelVertex>? vertices,
        IReadOnlyList<int> indices)
    {
        if (vertices is null || vertices.Count != 24 || indices.Count != 36)
        {
            return false;
        }

        for (int index = 0; index < indices.Count; index += 3)
        {
            VoxelVertex first = vertices[indices[index]];
            VoxelVertex second = vertices[indices[index + 1]];
            VoxelVertex third = vertices[indices[index + 2]];
            float3 firstEdge = second.Position - first.Position;
            float3 secondEdge = third.Position - first.Position;
            var cross = new float3(
                firstEdge.y * secondEdge.z - firstEdge.z * secondEdge.y,
                firstEdge.z * secondEdge.x - firstEdge.x * secondEdge.z,
                firstEdge.x * secondEdge.y - firstEdge.y * secondEdge.x);
            float normalDotCross =
                first.Normal.x * cross.x +
                first.Normal.y * cross.y +
                first.Normal.z * cross.z;

            if (normalDotCross >= 0.0f)
            {
                return false;
            }
        }

        return true;
    }

    private sealed class TestFixture : IDisposable
    {
        public TestFixture()
        {
            SimWorld = new();
            System = new(SimWorld);
            System.Initialize();
        }

        public SimWorld SimWorld { get; }
        public UpdateVoxelChunkMeshesSystem System { get; }

        public Entity CreateChunk(
            IVisualVoxelChunk? visualChunk,
            bool isDirty)
        {
            Entity entity = SimWorld.EcsWorld.CreateEntity();
            entity.Set(new VoxelChunk
            {
                Transform = double3xform.identity,
                VisualVoxelChunk = visualChunk,
            });

            if (isDirty)
            {
                entity.Set<Dirty>();
            }

            return entity;
        }

        public void Dispose()
        {
            System.Shutdown();
            SimWorld.Dispose();
        }
    }
}
