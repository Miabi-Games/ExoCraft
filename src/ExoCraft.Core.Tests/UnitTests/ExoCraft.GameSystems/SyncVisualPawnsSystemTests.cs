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
public class SyncVisualPawnsSystemTests
{
    // ─────────────────────────────────────────────────────────────────────────

    [Test]
    public void Test_001_UpdateMethod_ShouldSyncAllVisualPawnPositions()
    {
        using var fixture = new TestFixture();
        var firstPosition = new double3xform(
            (10.25, -20.5, 30.75),
            double3basis.identity,
            1.5);
        var secondPosition = new double3xform(
            (-40.125, 50.25, -60.5),
            new(
                0.0, 0.0, 1.0,
                0.0, 1.0, 0.0,
                -1.0, 0.0, 0.0),
            0.75);
        var firstVisualPawn = new Mock<IVisualPawn>();
        var secondVisualPawn = new Mock<IVisualPawn>();

        fixture.CreatePawn(firstPosition, firstVisualPawn.Object);
        fixture.CreatePawn(secondPosition, secondVisualPawn.Object);

        fixture.System.Update(1.0 / 60.0);

        fixture.VisualWorldMock.Verify(
            world => world.SyncPawn(firstVisualPawn.Object, firstPosition),
            Times.Once);
        fixture.VisualWorldMock.Verify(
            world => world.SyncPawn(secondVisualPawn.Object, secondPosition),
            Times.Once);
    }

    [Test]
    public void Test_002_UpdateMethod_ShouldSyncTheCurrentPawnPosition()
    {
        using var fixture = new TestFixture();
        var visualPawn = new Mock<IVisualPawn>();
        var pawnEntity = fixture.CreatePawn(
            double3xform.identity,
            visualPawn.Object);
        var expectedPosition = new double3xform(
            (100.0, 200.0, 300.0),
            double3basis.identity,
            2.0);

        pawnEntity.Get<Pawn>().Transform = expectedPosition;
        fixture.System.Update(1.0 / 60.0);

        fixture.VisualWorldMock.Verify(
            world => world.SyncPawn(visualPawn.Object, expectedPosition),
            Times.Once);
    }

    [Test]
    public void Test_003_UpdateMethod_ShouldAllowPawnWithoutVisualPawn()
    {
        using var fixture = new TestFixture();
        fixture.CreatePawn(double3xform.identity, visualPawn: null);

        Assert.That(
            () => fixture.System.Update(1.0 / 60.0),
            Throws.Nothing);
    }

    // ─────────────────────────────────────────────────────────────────────────

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
        public SyncVisualPawnsSystem System { get; }

        public Entity CreatePawn(
            double3xform position,
            IVisualPawn? visualPawn)
        {
            var pawn = new Pawn
            {
                Transform = position,
                VisualPawn = visualPawn,
            };
            var entity = SimWorld.EcsWorld.CreateEntity();
            entity.Set(pawn);

            return entity;
        }

        public void Dispose()
        {
            System.Shutdown();
            SimWorld.Dispose();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
}
