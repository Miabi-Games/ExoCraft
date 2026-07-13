using ExoCraft.Framework.Math;
using ExoCraft.Testing;

using NUnit.Framework;

using System.Diagnostics.CodeAnalysis;

namespace UnitTests.ExoCraft.Framework.Math;

[TestFixture, CommitStage, UnitTest]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
public class double3_Tests
{
    // We test NUnit's IsEqualTo assertion as a precondition because it uses the
    // default equality operator, which may be redefined for this type.

    [Test]
    public void Precondition_001_NUnit_IsEqualTo_ShouldBeTrueIfAllElementsAreEqual()
    {
        double3 value = new() { x = 1.0, y = 2.0, z = 3.0 };
        Assert.That(value, Is.EqualTo(new double3() { x = 1.0, y = 2.0, z = 3.0 }));
    }

    [Test]
    public void Precondition_002_NUnit_IsEqualTo_ShouldNotBeTrueIfAnyElementIsNotEqual()
    {
        double3 value = new() { x = 1.0, y = 2.0, z = 3.0 };

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value, Is.Not.EqualTo(new double3() { x = 0.0, y = 2.0, z = 3.0 }));
            Assert.That(value, Is.Not.EqualTo(new double3() { x = 1.0, y = 0.0, z = 3.0 }));
            Assert.That(value, Is.Not.EqualTo(new double3() { x = 1.0, y = 2.0, z = 0.0 }));
        }
    }

    [Test]
    public void Test_001_Ctor_ShouldSetElements()
    {
        double3 value = new(1.0, 2.0, 3.0);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value.x, Is.EqualTo(1.0));
            Assert.That(value.y, Is.EqualTo(2.0));
            Assert.That(value.z, Is.EqualTo(3.0));
        }
    }

    [Test]
    public void Test_002_ImplicitTupleConversion_ShouldSetElements()
    {
        double3 value = (1.0, 2.0, 3.0);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value.x, Is.EqualTo(1.0));
            Assert.That(value.y, Is.EqualTo(2.0));
            Assert.That(value.z, Is.EqualTo(3.0));
        }
    }

    [Test]
    public void Test_003_TupleDeconstruction_ShouldExtractElements()
    {
        double3 value = new() { x = 1.0, y = 2.0, z = 3.0 };
        (double x, double y, double z) = value;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(x, Is.EqualTo(1.0));
            Assert.That(y, Is.EqualTo(2.0));
            Assert.That(z, Is.EqualTo(3.0));
        }
    }

    [Test]
    public void Test_004_PositiveOperator_ShouldNotChangeAnyElement()
    {
        double3 initial = (1.0, 2.0, 3.0);
        double3 expected = (1.0, 2.0, 3.0);

        double3 actual = +initial;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_005_NegativeOperator_ShouldNegateAllElements()
    {
        double3 initial = (1.0, 2.0, 3.0);
        double3 expected = (-1.0, -2.0, -3.0);

        double3 actual = -initial;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_006_AdditionOperator_ShouldAddCorrespondingElements()
    {
        double3 v1 = (1.0, 4.0, 16.0);
        double3 v2 = (2.0, 8.0, 32.0);
        double3 expected = (3.0, 12.0, 48.0);

        double3 actual = v1 + v2;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_007_SubtractionOperator_ShouldSubtractCorrespondingElements()
    {
        double3 v1 = (1.0, 4.0, 16.0);
        double3 v2 = (2.0, 8.0, 32.0);
        double3 expected = (-1.0, -4.0, -16.0);

        double3 actual = v1 - v2;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_008_MultiplicationWithScalarOnRight_ShouldMultiplyEachElementByScalar()
    {
        double s = 2.0;
        double3 v = (3.0, 5.0, 7.0);
        double3 expected = (6.0, 10.0, 14.0);

        double3 actual = v * s;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_009_MultiplicationWithScalarOnLeft_ShouldMultiplyEachElementByScalar()
    {
        double s = 2.0;
        double3 v = (3.0, 5.0, 7.0);
        double3 expected = (6.0, 10.0, 14.0);

        double3 actual = s * v;

        Assert.That(actual, Is.EqualTo(expected));
    }
}
