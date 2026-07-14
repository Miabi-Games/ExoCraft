using ExoCraft.Framework.Math;
using ExoCraft.Testing;

using NUnit.Framework;

using System.Diagnostics.CodeAnalysis;

namespace UnitTests.ExoCraft.Framework.Math;

[TestFixture, CommitStage, UnitTest]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
public class double3xform_Tests
{
    [Test]
    public void Test_001_Ctor_ShouldSetElements()
    {
        double3 expected_position = new double3(14, 24, 34);
        double3basis expected_rotation = new double3basis(
                new double3(11, 21, 31),
                new double3(12, 22, 32),
                new double3(13, 23, 33));
        double expected_scale = 1.0;

        double3xform value = new(
            new double3(14, 24, 34),
            new double3basis(
                new double3(11, 21, 31),
                new double3(12, 22, 32),
                new double3(13, 23, 33)),
            1.0);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value.position, Is.EqualTo(expected_position));
            Assert.That(value.rotation, Is.EqualTo(expected_rotation));
            Assert.That(value.scale, Is.EqualTo(expected_scale));
        }
    }

    [Test]
    public void Test_002_TransformPositionUsingIdentity_ShouldNotChangePosition()
    {
        double3 position = (2.0, -3.0, 4.0);

        double3 result = double3xform.identity.transform_position(position);

        Assert.That(result, Is.EqualTo(position));
    }

    [Test]
    public void Test_003_TransformPosition_ShouldApplyScaleRotationAndTranslation()
    {
        var rotation = double3basis.identity;
        rotation.rotate_parent_y(System.Math.PI / 2.0);
        var transform = new double3xform(
            position: (10.0, 20.0, 30.0),
            rotation,
            scale: 2.0);

        double3 result = transform.transform_position((1.0, 2.0, 3.0));

        const double tolerance = 1e-12;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.x, Is.EqualTo(16.0).Within(tolerance));
            Assert.That(result.y, Is.EqualTo(24.0).Within(tolerance));
            Assert.That(result.z, Is.EqualTo(28.0).Within(tolerance));
        }
    }
}
