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
public class CameraLifecycleSystemTests
{
    [Test]
    public void Test_001_InitializeMethod_ShouldCreateCameraEntity()
    {
        using var fixture = new TestFixture(initializeSystem: false);

        fixture.InitializeSystem();

        Assert.That(fixture.Cameras.Count, Is.EqualTo(1));
    }

    [Test]
    public void Test_002_CreatedCameraEntity_ShouldNotBePawn()
    {
        using var fixture = new TestFixture();
        var entity = fixture.GetCameraEntityCreated();

        Assert.That(entity.Has<Pawn>(), Is.False);
    }

    [Test]
    public void Test_003_CreatedCameraEntity_ShouldHaveInitialTransform()
    {
        using var fixture = new TestFixture();
        var entity = fixture.GetCameraEntityCreated();
        var expectedRotation = double3basis.identity;
        expectedRotation.rotate_local_x(-System.Math.PI / 6.0);
        var expectedTransform = new double3xform(
            (0.0, 6.0, 13.0),
            expectedRotation,
            1.0);

        Assert.That(
            entity.Get<Camera>().Transform,
            Is.EqualTo(expectedTransform));
    }

    [Test]
    public void Test_004_ShutdownMethod_ShouldDestroyCameraEntity()
    {
        using var fixture = new TestFixture();

        fixture.ShutdownSystem();

        Assert.That(fixture.Cameras, Is.Empty);
    }

    private sealed class TestFixture : IDisposable
    {
        public TestFixture(bool initializeSystem = true)
        {
            SimWorld = new();
            Cameras = SimWorld.EcsWorld
                .GetEntities()
                .With<Camera>()
                .AsSet();
            System = new(SimWorld);

            if (initializeSystem) InitializeSystem();
        }

        public SimWorld SimWorld { get; }
        public EntitySet Cameras { get; }
        public CameraLifecycleSystem System { get; }

        public void InitializeSystem()
        {
            System.Initialize();
            _isInitialized = true;
        }

        public void ShutdownSystem()
        {
            System.Shutdown();
            _isInitialized = false;
        }

        public Entity GetCameraEntityCreated()
        {
            Assert.That(Cameras.Count, Is.EqualTo(1));
            return Cameras.GetEntities()[0];
        }

        public void Dispose()
        {
            if (_isInitialized) System.Shutdown();

            Cameras.Dispose();
            SimWorld.Dispose();
        }

        private bool _isInitialized;
    }
}
