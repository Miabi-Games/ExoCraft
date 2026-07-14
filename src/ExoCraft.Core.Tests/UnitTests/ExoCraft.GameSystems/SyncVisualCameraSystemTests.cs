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
public class SyncVisualCameraSystemTests
{
    [Test]
    public void Test_001_UpdateMethod_ShouldSyncCameraTransform()
    {
        using var fixture = new TestFixture();
        var expectedTransform = new double3xform(
            (10.25, -20.5, 30.75),
            double3basis.identity,
            1.0);
        fixture.CreateCamera(expectedTransform);

        fixture.System.Update(1.0 / 60.0);

        fixture.VisualWorldMock.VerifySet(
            world => world.CameraTransform = expectedTransform,
            Times.Once);
    }

    [Test]
    public void Test_002_UpdateMethod_ShouldSyncCurrentCameraTransform()
    {
        using var fixture = new TestFixture();
        var cameraEntity = fixture.CreateCamera(double3xform.identity);
        var expectedTransform = new double3xform(
            (-100.0, 200.0, -300.0),
            new(
                0.0, 0.0, 1.0,
                0.0, 1.0, 0.0,
                -1.0, 0.0, 0.0),
            1.0);

        cameraEntity.Get<Camera>().Transform = expectedTransform;
        fixture.System.Update(1.0 / 60.0);

        fixture.VisualWorldMock.VerifySet(
            world => world.CameraTransform = expectedTransform,
            Times.Once);
    }

    [Test]
    public void Test_003_UpdateMethod_ShouldAllowNoCamera()
    {
        using var fixture = new TestFixture();

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
        public SyncVisualCameraSystem System { get; }

        public Entity CreateCamera(double3xform transform)
        {
            var entity = SimWorld.EcsWorld.CreateEntity();
            entity.Set(new Camera { Transform = transform });
            return entity;
        }

        public void Dispose()
        {
            System.Shutdown();
            SimWorld.Dispose();
        }
    }
}
