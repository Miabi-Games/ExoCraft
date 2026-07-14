using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.InputProviders;
using ExoCraft.Framework.Math;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.GameSystems;
using ExoCraft.Testing;

using Moq;

using NUnit.Framework;

using System;

namespace UnitTests.ExoCraft.GameSystems;

[TestFixture, CommitStage, UnitTest]
public class UpdatePlayerInputSystemTests
{
    [Test]
    public void Test_001_InitializeMethod_ShouldAddPlayerInputToWorld()
    {
        using var fixture = new TestFixture(initializeSystem: false);

        fixture.System.Initialize();

        Assert.That(fixture.EcsWorld.Has<PlayerInput>(), Is.True);
    }

    [Test]
    public void Test_002_UpdateMethod_ShouldStoreInputOnWorld()
    {
        using var fixture = new TestFixture();
        var expectedMovement = new float3(1.0f, -0.5f, 0.25f);
        var expectedRotation = new float3(-0.25f, 0.75f, 0.5f);
        fixture.SetInput(expectedMovement, expectedRotation);

        fixture.System.Update(0.5);

        PlayerInput playerInput = fixture.EcsWorld.Get<PlayerInput>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(playerInput.Movement, Is.EqualTo(expectedMovement));
            Assert.That(playerInput.Rotation, Is.EqualTo(expectedRotation));
        }
    }

    [Test]
    public void Test_003_ShutdownMethod_ShouldRemovePlayerInputFromWorld()
    {
        using var fixture = new TestFixture();

        fixture.System.Shutdown();

        Assert.That(fixture.EcsWorld.Has<PlayerInput>(), Is.False);
    }

    private sealed class TestFixture : IDisposable
    {
        public TestFixture(bool initializeSystem = true)
        {
            SimWorld = new();
            InputProvider = new();
            System = new(SimWorld, InputProvider.Object);

            if (initializeSystem) System.Initialize();
        }

        public SimWorld SimWorld { get; }
        public World EcsWorld => SimWorld.EcsWorld;
        public Mock<IInputProvider> InputProvider { get; }
        public UpdatePlayerInputSystem System { get; }

        public void SetInput(float3 movement, float3 rotation)
        {
            InputProvider
                .SetupGet(input => input.MovementInput)
                .Returns(movement);
            InputProvider
                .SetupGet(input => input.RotationInput)
                .Returns(rotation);
        }

        public void Dispose()
        {
            SimWorld.Dispose();
        }
    }
}
