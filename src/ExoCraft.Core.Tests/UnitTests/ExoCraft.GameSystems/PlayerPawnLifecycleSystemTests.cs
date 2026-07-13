using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;
using ExoCraft.GameSystems;
using ExoCraft.Testing;

using Moq;

using NUnit.Framework;

using System;

namespace UnitTests.ExoCraft.GameSystems;

#pragma warning disable NUnit2045 // Use Assert.EnterMultipleScope or Assert.Multiple

[TestFixture, CommitStage, UnitTest]
public class PlayerPawnLifecycleSystemTests
{
    // ─────────────────────────────────────────────────────────────────────────

    [Test]
    public void Test_001_InitializeMethod_ShouldCreatePlayerPawnEntity()
    {
        using var fixture = CreateTestFixture(initializeSystem: false);

        fixture.System.Initialize();

        Assert.That(fixture.PlayerPawns.Count, Is.EqualTo(1));
    }

    [Test]
    public void Test_002_CreatedPlayerEntity_ShouldHaveCreatedVisualPawn()
    {
        using var fixture = CreateTestFixture(initializeSystem: true);
        var entity = fixture.GetPlayerEntityCreated();

        Assert.That(entity.Has<Pawn>(), Is.True);
        Assert.That(entity.Get<Pawn>()?.VisualPawn, Is.SameAs(fixture.ExpectedVisualPawn));
    }

    [Test]
    public void Test_003_ShutdownMethod_ShouldDestroyPlayerPawnEntity()
    {
        using var fixture = CreateTestFixture();

        fixture.System.Shutdown();

        Assert.That(fixture.PlayerPawns, Is.Empty);
    }

    [Test]
    public void Test_004_ShutdownMethod_ShouldFreeTheVisualPawn()
    {
        var fixture = CreateTestFixture();

        fixture.System.Shutdown();

        fixture.ExpectedVisualPawnMock.Verify(mock => mock.Free(), Times.Once);
    }

    // ─────────────────────────────────────────────────────────────────────────

    private static TestFixture CreateTestFixture(bool initializeSystem = true)
    {
        SimWorld? simWorld = null;
        EntitySet? playerPawns = null;

        try
        {
            var expectedPawn = new Mock<IVisualPawn>();
            var visualWorld = new Mock<IVisualWorld>();

            visualWorld
                .Setup(w => w.CreatePlayerPawn())
                .Returns(expectedPawn.Object);

            simWorld = new SimWorld();
            var ecsWorld = simWorld.EcsWorld;

            playerPawns = ecsWorld
                .GetEntities()
                .With<PlayerPawn>()
                .AsSet();

            var system = new PlayerPawnLifecycleSystem(simWorld, visualWorld.Object);

            if (initializeSystem)
            {
                system.Initialize();
                Assert.That(playerPawns.Count, Is.EqualTo(1));
            }

            return new()
            {
                VisualWorld = visualWorld.Object,
                ExpectedVisualPawn = expectedPawn.Object,
                ExpectedVisualPawnMock = expectedPawn,
                SimWorld = simWorld,
                EcsWorld = ecsWorld,
                PlayerPawns = playerPawns,
                System = system,
            };
        }
        catch (Exception)
        {
            playerPawns?.Dispose();
            simWorld?.Dispose();
            throw;
        }
    }

    private class TestFixture : IDisposable
    {
        public required IVisualWorld VisualWorld { get; init; }
        public required IVisualPawn ExpectedVisualPawn { get; init; }

        public required Mock<IVisualPawn> ExpectedVisualPawnMock { get; init; }

        public required SimWorld SimWorld { get; init; }
        public required World EcsWorld { get; init; }

        public required EntitySet PlayerPawns { get; init; }

        public required PlayerPawnLifecycleSystem System { get; init; }

        public void Dispose()
        {
            PlayerPawns.Dispose();
            SimWorld.Dispose();
        }

        public Entity GetPlayerEntityCreated()
        {
            Assert.That(PlayerPawns.Count, Is.EqualTo(1));
            return PlayerPawns.GetEntities()[0];
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
}

#pragma warning restore NUnit2045 // Use Assert.EnterMultipleScope or Assert.Multiple
