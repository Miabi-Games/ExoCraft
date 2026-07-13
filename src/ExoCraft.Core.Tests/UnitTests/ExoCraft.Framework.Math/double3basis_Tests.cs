using ExoCraft.Framework.Math;
using ExoCraft.Testing;

using NUnit.Framework;

using System.Diagnostics.CodeAnalysis;

namespace UnitTests.ExoCraft.Framework.Math;

[TestFixture, CommitStage, UnitTest]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
internal class double3basis_Tests
{
    // We test NUnit's IsEqualTo assertion as a precondition because it uses the
    // default equality operator, which may be redefined for this type.

    [Test]
    public void Precondition_001_NUnit_IsEqualTo_ShouldBeTrueIfAllElementsAreEqual()
    {
        double3basis value1 = new()
        {
            x = (11, 21, 31),
            y = (12, 22, 32),
            z = (13, 23, 33),
        };

        double3basis value2 = new()
        {
            x = (11, 21, 31),
            y = (12, 22, 32),
            z = (13, 23, 33),
        };

        Assert.That(value1, Is.EqualTo(value2));
    }

    [Test]
    public void Precondition_002_NUnit_IsEqualTo_ShouldNotBeTrueIfAnyElementIsNotEqual()
    {
        double3basis value1 = new()
        {
            x = (11, 21, 31),
            y = (12, 22, 32),
            z = (13, 23, 33),
        };

        using (Assert.EnterMultipleScope())
        {
            for (int i = 0; i < 9; i++)
            {
                double3basis value2 = new()
                {
                    x = (11, 21, 31),
                    y = (12, 22, 32),
                    z = (13, 23, 33),
                };

                switch (i)
                {
                case 0: value2.x.x = 0; break;
                case 1: value2.x.y = 0; break;
                case 2: value2.x.z = 0; break;

                case 3: value2.y.x = 0; break;
                case 4: value2.y.y = 0; break;
                case 5: value2.y.z = 0; break;

                case 6: value2.z.x = 0; break;
                case 7: value2.z.y = 0; break;
                case 8: value2.z.z = 0; break;
                }

                Assert.That(value1, Is.Not.EqualTo(value2));
            }
        }
    }

    [Test]
    public void Test_001_CtorUsingVectors_ShouldSetVectorElements()
    {
        double3 x = (11, 21, 31);
        double3 y = (12, 22, 32);
        double3 z = (13, 23, 33);

        double3basis value = new(x, y, z);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value.x, Is.EqualTo(x));
            Assert.That(value.y, Is.EqualTo(y));
            Assert.That(value.z, Is.EqualTo(z));
        }
    }

    [Test]
    public void Test_002_BasisVectors_ShouldBeStoredAsColumns()
    {
        // notice the elements match the position in standard matrix notation
        double3 x = (11, 21, 31);
        double3 y = (12, 22, 32);
        double3 z = (13, 23, 33);

        double3basis value = new(x, y, z);

        using (Assert.EnterMultipleScope())
        {
            // Note: Matrix element properties use standard matrix notation
            // where the row number comes first. So `e23` is the element in
            // column 3 of row 2. This is not the same as how double3basis
            // stores its elements, which is column major. So this test is
            // really more about the element properties.

            Assert.That(value.e11, Is.EqualTo(11));
            Assert.That(value.e12, Is.EqualTo(12));
            Assert.That(value.e13, Is.EqualTo(13));

            Assert.That(value.e21, Is.EqualTo(21));
            Assert.That(value.e22, Is.EqualTo(22));
            Assert.That(value.e23, Is.EqualTo(23));

            Assert.That(value.e31, Is.EqualTo(31));
            Assert.That(value.e32, Is.EqualTo(32));
            Assert.That(value.e33, Is.EqualTo(33));
        }
    }

    [Test]
    public void Test_003_CtorUsingElements_ShouldSetElementsCorrectly()
    {
        double3basis value = new(
            11, 12, 13,
            21, 22, 23,
            31, 32, 33);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value.e11, Is.EqualTo(11));
            Assert.That(value.e12, Is.EqualTo(12));
            Assert.That(value.e13, Is.EqualTo(13));

            Assert.That(value.e21, Is.EqualTo(21));
            Assert.That(value.e22, Is.EqualTo(22));
            Assert.That(value.e23, Is.EqualTo(23));

            Assert.That(value.e31, Is.EqualTo(31));
            Assert.That(value.e32, Is.EqualTo(32));
            Assert.That(value.e33, Is.EqualTo(33));
        }
    }

    [Test]
    public void Test_004_ImplicitTupleConversion_ShouldSetElements()
    {
        double3basis value = new double3basis(
            11, 12, 13,
            21, 22, 23,
            31, 32, 33);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value.x, Is.EqualTo(new double3(11, 21, 31)));
            Assert.That(value.y, Is.EqualTo(new double3(12, 22, 32)));
            Assert.That(value.z, Is.EqualTo(new double3(13, 23, 33)));
        }
    }

    [Test]
    public void Test_005_TupleDeconstruction_ShouldExtractElements()
    {
        double3basis value = new double3basis(
            11, 12, 13,
            21, 22, 23,
            31, 32, 33);

        (double3 x, double3 y, double3 z) = value;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value.x, Is.EqualTo(new double3(11, 21, 31)));
            Assert.That(value.y, Is.EqualTo(new double3(12, 22, 32)));
            Assert.That(value.z, Is.EqualTo(new double3(13, 23, 33)));
        }
    }
}
