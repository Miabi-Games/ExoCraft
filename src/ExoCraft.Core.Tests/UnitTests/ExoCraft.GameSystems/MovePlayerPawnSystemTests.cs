using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.Math;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.GameSystems;
using ExoCraft.Testing;

using NUnit.Framework;

using System;

namespace UnitTests.ExoCraft.GameSystems;

[TestFixture, CommitStage, UnitTest]
public class MovePlayerPawnSystemTests
{
    [Test]
    public void Test_001_UpdateMethod_ShouldMovePlayerPawnUsingInputAndDelta()
    {
        using var fixture = new TestFixture();
        var pawnEntity = fixture.CreatePlayerPawn();
        fixture.SetInput(
            movement: (1.0f, -0.5f, -0.25f),
            rotation: float3.zero);

        fixture.System.Update(0.5);

        Assert.That(
            pawnEntity.Get<Pawn>().Transform.position,
            Is.EqualTo(new double3(2.5, 0.0, -0.625)));
    }

    [Test]
    public void Test_002_UpdateMethod_ShouldMoveRelativeToPawnRotation()
    {
        using var fixture = new TestFixture();
        var position = double3xform.identity;
        position.rotation.rotate_parent_y(System.Math.PI / 2.0);
        var pawnEntity = fixture.CreatePlayerPawn(position);
        fixture.SetInput(
            movement: float3.forward,
            rotation: float3.zero);

        fixture.System.Update(1.0);

        AssertDouble3IsApproximatelyEqual(
            pawnEntity.Get<Pawn>().Transform.position,
            new double3(-5.0, 0.0, 0.0));
    }

    [Test]
    public void Test_003_UpdateMethod_ShouldRotatePlayerPawnUsingInputAndDelta()
    {
        using var fixture = new TestFixture();
        var pawnEntity = fixture.CreatePlayerPawn();
        fixture.SetInput(
            movement: float3.zero,
            rotation: (0.0f, 0.75f, 0.0f));

        fixture.System.Update(1.0);

        var expectedRotation = double3basis.identity;
        expectedRotation.rotate_parent_y(System.Math.PI / 2.0);
        AssertBasisIsApproximatelyEqual(
            pawnEntity.Get<Pawn>().Transform.rotation,
            expectedRotation);
    }

    [Test]
    public void Test_004_UpdateMethod_ShouldNotMoveNonPlayerPawn()
    {
        using var fixture = new TestFixture();
        var pawnEntity = fixture.CreatePawn();
        fixture.SetInput(
            movement: float3.forward,
            rotation: (0.0f, 1.0f, 0.0f));

        fixture.System.Update(1.0);

        Assert.That(
            pawnEntity.Get<Pawn>().Transform,
            Is.EqualTo(double3xform.identity));
    }

    [Test]
    public void Test_005_UpdateMethod_ShouldIgnoreVerticalMovementInput()
    {
        using var fixture = new TestFixture();
        var pawnEntity = fixture.CreatePlayerPawn();
        fixture.SetInput(
            movement: float3.up,
            rotation: float3.zero);

        fixture.System.Update(0.25);

        Assert.That(
            pawnEntity.Get<Pawn>().Transform.position,
            Is.EqualTo(double3.zero));
    }

    private static void AssertBasisIsApproximatelyEqual(
        double3basis actual,
        double3basis expected)
    {
        AssertDouble3IsApproximatelyEqual(actual.x, expected.x);
        AssertDouble3IsApproximatelyEqual(actual.y, expected.y);
        AssertDouble3IsApproximatelyEqual(actual.z, expected.z);
    }

    private static void AssertDouble3IsApproximatelyEqual(
        double3 actual,
        double3 expected)
    {
        const double tolerance = 1e-12;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.x, Is.EqualTo(expected.x).Within(tolerance));
            Assert.That(actual.y, Is.EqualTo(expected.y).Within(tolerance));
            Assert.That(actual.z, Is.EqualTo(expected.z).Within(tolerance));
        }
    }

    private sealed class TestFixture : IDisposable
    {
        public TestFixture()
        {
            SimWorld = new();
            SimWorld.EcsWorld.Set<PlayerInput>();
            System = new(SimWorld);
            System.Initialize();
        }

        public SimWorld SimWorld { get; }
        public MovePlayerPawnSystem System { get; }

        public Entity CreatePawn()
        {
            return CreatePawn(double3xform.identity, isPlayer: false);
        }

        public Entity CreatePlayerPawn()
        {
            return CreatePawn(double3xform.identity, isPlayer: true);
        }

        public Entity CreatePlayerPawn(double3xform position)
        {
            return CreatePawn(position, isPlayer: true);
        }

        public void SetInput(float3 movement, float3 rotation)
        {
            ref var playerInput = ref SimWorld.EcsWorld.Get<PlayerInput>();

            playerInput.Movement = movement;
            playerInput.Rotation = rotation;
        }

        public void Dispose()
        {
            System.Shutdown();
            SimWorld.Dispose();
        }

        private Entity CreatePawn(double3xform position, bool isPlayer)
        {
            var pawn = new Pawn
            {
                Transform = position,
            };
            Entity entity = SimWorld.EcsWorld.CreateEntity();
            entity.Set(pawn);

            if (isPlayer) entity.Set<PlayerPawn>();

            return entity;
        }
    }
}
