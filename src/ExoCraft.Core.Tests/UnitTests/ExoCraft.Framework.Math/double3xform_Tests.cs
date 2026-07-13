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
}
