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
public class VoxelChunkLifecycleSystemTests
{
    [Test]
    public void Test_001_InitializeMethod_ShouldCreateSixteenVoxelChunks()
    {
        using var fixture = new TestFixture(initializeSystem: false);

        fixture.System.Initialize();

        Assert.That(fixture.VoxelChunks.Count, Is.EqualTo(16));
        fixture.VisualWorldMock.Verify(
            world => world.CreateVoxelChunk(),
            Times.Exactly(16));
    }

    [Test]
    public void Test_002_CreatedChunks_ShouldFormCenteredFourByFourArray()
    {
        using var fixture = new TestFixture();
        var positions = new HashSet<double3>();

        foreach (Entity entity in fixture.VoxelChunks.GetEntities())
        {
            VoxelChunk chunk = entity.Get<VoxelChunk>();

            positions.Add(chunk.Transform.position);
            Assert.That(chunk.Transform.rotation, Is.EqualTo(double3basis.identity));
            Assert.That(chunk.Transform.scale, Is.EqualTo(1.0));
        }

        double[] expectedCoordinates = { -12.0, -4.0, 4.0, 12.0 };

        foreach (double z in expectedCoordinates)
        {
            foreach (double x in expectedCoordinates)
            {
                Assert.That(positions, Does.Contain(new double3(x, -4.0, z)));
            }
        }
    }

    [Test]
    public void Test_003_CreatedChunks_ShouldInitiallyBeDirty()
    {
        using var fixture = new TestFixture();

        foreach (Entity entity in fixture.VoxelChunks.GetEntities())
        {
            Assert.That(entity.Has<Dirty>(), Is.True);
        }
    }

    [Test]
    public void Test_004_ShutdownMethod_ShouldDestroyAllVoxelChunks()
    {
        using var fixture = new TestFixture();

        fixture.System.Shutdown();

        Assert.That(fixture.VoxelChunks, Is.Empty);
        foreach (Mock<IVisualVoxelChunk> visualChunk in fixture.VisualChunks)
        {
            visualChunk.Verify(chunk => chunk.Free(), Times.Once);
        }
    }

    private sealed class TestFixture : IDisposable
    {
        public TestFixture(bool initializeSystem = true)
        {
            SimWorld = new();
            VisualWorldMock = new();
            VisualChunks = new();
            VoxelChunks = SimWorld.EcsWorld.GetEntities()
                .With<VoxelChunk>()
                .AsSet();

            VisualWorldMock
                .Setup(world => world.CreateVoxelChunk())
                .Returns(() =>
                {
                    var visualChunk = new Mock<IVisualVoxelChunk>();
                    VisualChunks.Add(visualChunk);
                    return visualChunk.Object;
                });

            System = new(SimWorld, VisualWorldMock.Object);

            if (initializeSystem)
            {
                System.Initialize();
            }
        }

        public SimWorld SimWorld { get; }
        public Mock<IVisualWorld> VisualWorldMock { get; }
        public List<Mock<IVisualVoxelChunk>> VisualChunks { get; }
        public EntitySet VoxelChunks { get; }
        public VoxelChunkLifecycleSystem System { get; }

        public void Dispose()
        {
            VoxelChunks.Dispose();
            SimWorld.Dispose();
        }
    }
}
