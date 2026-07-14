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

namespace UnitTests.ExoCraft.GameSystems;

[TestFixture, CommitStage, UnitTest]
public class SyncVisualVoxelChunksSystemTests
{
    [Test]
    public void Test_001_UpdateMethod_ShouldSyncAllVisualChunkPositions()
    {
        using var fixture = new TestFixture();
        var firstTransform = new double3xform(
            (10.0, 20.0, 30.0),
            double3basis.identity,
            1.0);
        var secondTransform = new double3xform(
            (-40.0, -50.0, -60.0),
            double3basis.identity,
            2.0);
        var firstVisualChunk = new Mock<IVisualVoxelChunk>();
        var secondVisualChunk = new Mock<IVisualVoxelChunk>();

        fixture.CreateChunk(firstTransform, firstVisualChunk.Object);
        fixture.CreateChunk(secondTransform, secondVisualChunk.Object);

        fixture.System.Update(1.0 / 60.0);

        fixture.VisualWorldMock.Verify(world => world.SyncVoxelChunk(
            firstVisualChunk.Object,
            firstTransform), Times.Once);
        fixture.VisualWorldMock.Verify(world => world.SyncVoxelChunk(
            secondVisualChunk.Object,
            secondTransform), Times.Once);
    }

    [Test]
    public void Test_002_UpdateMethod_ShouldSyncCurrentChunkPosition()
    {
        using var fixture = new TestFixture();
        var visualChunk = new Mock<IVisualVoxelChunk>();
        Entity entity = fixture.CreateChunk(
            double3xform.identity,
            visualChunk.Object);
        var expectedTransform = new double3xform(
            (100.0, 200.0, 300.0),
            double3basis.identity,
            1.0);

        entity.Get<VoxelChunk>().Transform = expectedTransform;
        fixture.System.Update(1.0 / 60.0);

        fixture.VisualWorldMock.Verify(world => world.SyncVoxelChunk(
            visualChunk.Object,
            expectedTransform), Times.Once);
    }

    [Test]
    public void Test_003_UpdateMethod_ShouldAllowChunkWithoutVisualChunk()
    {
        using var fixture = new TestFixture();
        fixture.CreateChunk(double3xform.identity, visualChunk: null);

        Assert.That(
            () => fixture.System.Update(1.0 / 60.0),
            Throws.Nothing);
    }

    private sealed class TestFixture : IDisposable
    {
        public TestFixture()
        {
            SimWorld = new();
            VisualWorldMock = new();
            System = new(SimWorld, VisualWorldMock.Object);
            System.Initialize();
        }

        public SimWorld SimWorld { get; }
        public Mock<IVisualWorld> VisualWorldMock { get; }
        public SyncVisualVoxelChunksSystem System { get; }

        public Entity CreateChunk(
            double3xform transform,
            IVisualVoxelChunk? visualChunk)
        {
            var chunk = new VoxelChunk
            {
                Transform = transform,
                VisualVoxelChunk = visualChunk,
            };
            Entity entity = SimWorld.EcsWorld.CreateEntity();

            entity.Set(chunk);
            return entity;
        }

        public void Dispose()
        {
            System.Shutdown();
            SimWorld.Dispose();
        }
    }
}
