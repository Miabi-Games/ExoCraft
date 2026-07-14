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
public class FollowChaseCamSystemTests
{
    [Test]
    public void Test_001_UpdateMethod_ShouldPositionCameraBehindLookAtPoint()
    {
        using var fixture = new TestFixture();
        Entity target = fixture.CreateTarget(
            double3xform.identity,
            (0.0, 2.0, -0.4));
        Entity camera = fixture.CreateChaseCam(target, 2.0, 0.0);

        fixture.System.Update(0.0);

        Assert.That(
            camera.Get<Camera>().Transform,
            Is.EqualTo(new double3xform(
                (0.0, 2.0, 1.6),
                double3basis.identity,
                1.0)));
    }

    [Test]
    public void Test_002_UpdateMethod_ShouldFollowTransformedTarget()
    {
        using var fixture = new TestFixture();
        var targetTransform = new double3xform(
            position: (10.0, 20.0, 30.0),
            rotation: double3basis.identity,
            scale: 2.0);
        targetTransform.rotation.rotate_parent_y(System.Math.PI / 2.0);
        Entity target = fixture.CreateTarget(
            targetTransform,
            (0.0, 2.0, -0.4));
        Entity camera = fixture.CreateChaseCam(target, 2.0, 0.0);

        fixture.System.Update(0.0);

        double3xform result = camera.Get<Camera>().Transform;
        AssertDouble3IsApproximatelyEqual(
            result.position,
            (11.2, 24.0, 30.0));
        AssertBasisIsApproximatelyEqual(
            result.rotation,
            targetTransform.rotation);
    }

    [Test]
    public void Test_003_UpdateMethod_ShouldApplyPitchToCameraOffsetAndRotation()
    {
        using var fixture = new TestFixture();
        Entity target = fixture.CreateTarget(
            double3xform.identity,
            double3.zero);
        Entity camera = fixture.CreateChaseCam(
            target,
            distance: 2.0,
            pitch: -System.Math.PI / 2.0);

        fixture.System.Update(0.0);

        var expectedRotation = double3basis.identity;
        expectedRotation.rotate_local_x(-System.Math.PI / 2.0);
        double3xform result = camera.Get<Camera>().Transform;
        AssertDouble3IsApproximatelyEqual(result.position, (0.0, 2.0, 0.0));
        AssertBasisIsApproximatelyEqual(result.rotation, expectedRotation);
    }

    [Test]
    public void Test_004_UpdateMethod_ShouldUseCurrentChaseCamValues()
    {
        using var fixture = new TestFixture();
        Entity originalTarget = fixture.CreateTarget(
            double3xform.identity,
            double3.zero);
        var replacementTransform = double3xform.identity;
        replacementTransform.position = (5.0, 0.0, 0.0);
        Entity replacementTarget = fixture.CreateTarget(
            replacementTransform,
            double3.zero);
        Entity camera = fixture.CreateChaseCam(originalTarget, 2.0, 0.0);
        ref ChaseCam chaseCam = ref camera.Get<ChaseCam>();
        chaseCam.Target = replacementTarget;
        chaseCam.Distance = 3.0;

        fixture.System.Update(0.0);

        Assert.That(
            camera.Get<Camera>().Transform.position,
            Is.EqualTo(new double3(5.0, 0.0, 3.0)));
    }

    [Test]
    public void Test_005_UpdateMethod_ShouldIgnoreInvalidTarget()
    {
        using var fixture = new TestFixture();
        Entity target = fixture.CreateTarget(
            double3xform.identity,
            double3.zero);
        Entity camera = fixture.CreateChaseCam(target, 2.0, 0.0);
        double3xform initialTransform = camera.Get<Camera>().Transform;
        target.Dispose();

        fixture.System.Update(0.0);

        Assert.That(
            camera.Get<Camera>().Transform,
            Is.EqualTo(initialTransform));
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
            System = new(SimWorld);
            System.Initialize();
        }

        public SimWorld SimWorld { get; }
        public FollowChaseCamSystem System { get; }

        public Entity CreateTarget(
            double3xform transform,
            double3 lookAtPosition)
        {
            Entity entity = SimWorld.EcsWorld.CreateEntity();
            entity.Set(new Pawn { Transform = transform });
            entity.Set(new CameraLookAt { Position = lookAtPosition });
            return entity;
        }

        public Entity CreateChaseCam(
            Entity target,
            double distance,
            double pitch)
        {
            Entity entity = SimWorld.EcsWorld.CreateEntity();
            entity.Set(new Camera
            {
                Transform = new(
                    position: (0.0, 6.0, 13.0),
                    rotation: double3basis.identity,
                    scale: 1.0),
            });
            entity.Set(new ChaseCam
            {
                Target = target,
                Distance = distance,
                Pitch = pitch,
            });
            return entity;
        }

        public void Dispose()
        {
            System.Shutdown();
            SimWorld.Dispose();
        }
    }
}
