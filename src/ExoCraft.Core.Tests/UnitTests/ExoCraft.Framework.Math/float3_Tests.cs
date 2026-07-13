using ExoCraft.Framework.Math;
using ExoCraft.Testing;

using NUnit.Framework;

using System.Diagnostics.CodeAnalysis;

namespace UnitTests.ExoCraft.Framework.Math;

[TestFixture, CommitStage, UnitTest]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
public class float3_Tests
{
    // We test NUnit's IsEqualTo assertion as a precondition because it uses the
    // default equality operator, which may be redefined for this type.

    [Test]
    public void Precondition_001_NUnit_IsEqualTo_ShouldBeTrueIfAllElementsAreEqual()
    {
        float3 value = new() { x = 1f, y = 2f, z = 3f };
        Assert.That(value, Is.EqualTo(new float3() { x = 1f, y = 2f, z = 3f }));
    }

    [Test]
    public void Precondition_002_NUnit_IsEqualTo_ShouldNotBeTrueIfAnyElementIsNotEqual()
    {
        float3 value = new() { x = 1f, y = 2f, z = 3f };

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value, Is.Not.EqualTo(new float3() { x = 0f, y = 2f, z = 3f }));
            Assert.That(value, Is.Not.EqualTo(new float3() { x = 1f, y = 0f, z = 3f }));
            Assert.That(value, Is.Not.EqualTo(new float3() { x = 1f, y = 2f, z = 0f }));
        }
    }

    [Test]
    public void Test_001_Ctor_ShouldSetElements()
    {
        float3 value = new(1f, 2f, 3f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value.x, Is.EqualTo(1f));
            Assert.That(value.y, Is.EqualTo(2f));
            Assert.That(value.z, Is.EqualTo(3f));
        }
    }

    [Test]
    public void Test_002_ImplicitTupleConversion_ShouldSetElements()
    {
        float3 value = (1f, 2f, 3f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value.x, Is.EqualTo(1f));
            Assert.That(value.y, Is.EqualTo(2f));
            Assert.That(value.z, Is.EqualTo(3f));
        }
    }

    [Test]
    public void Test_003_TupleDeconstruction_ShouldExtractElements()
    {
        float3 value = new() { x = 1f, y = 2f, z = 3f };
        (float x, float y, float z) = value;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(x, Is.EqualTo(1f));
            Assert.That(y, Is.EqualTo(2f));
            Assert.That(z, Is.EqualTo(3f));
        }
    }

    [Test]
    public void Test_004_PositiveOperator_ShouldNotChangeAnyElement()
    {
        float3 initial = (1f, 2f, 3f);
        float3 expected = (1f, 2f, 3f);

        float3 actual = +initial;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_005_NegativeOperator_ShouldNegateAllElements()
    {
        float3 initial = (1f, 2f, 3f);
        float3 expected = (-1f, -2f, -3f);

        float3 actual = -initial;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_006_AdditionOperator_ShouldAddCorrespondingElements()
    {
        float3 v1 = (1f, 4f, 16f);
        float3 v2 = (2f, 8f, 32f);
        float3 expected = (3f, 12f, 48f);

        float3 actual = v1 + v2;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_007_SubtractionOperator_ShouldSubtractCorrespondingElements()
    {
        float3 v1 = (1f, 4f, 16f);
        float3 v2 = (2f, 8f, 32f);
        float3 expected = (-1f, -4f, -16f);

        float3 actual = v1 - v2;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_008_MultiplicationWithScalarOnRight_ShouldMultiplyEachElementByScalar()
    {
        float s = 2f;
        float3 v = (3f, 5f, 7f);
        float3 expected = (6f, 10f, 14f);

        float3 actual = v * s;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_009_MultiplicationWithScalarOnLeft_ShouldMultiplyEachElementByScalar()
    {
        float s = 2f;
        float3 v = (3f, 5f, 7f);
        float3 expected = (6f, 10f, 14f);

        float3 actual = s * v;

        Assert.That(actual, Is.EqualTo(expected));
    }
}
