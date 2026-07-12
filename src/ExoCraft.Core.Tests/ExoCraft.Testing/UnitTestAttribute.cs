using NUnit.Framework;

namespace ExoCraft.Testing;

public class UnitTestAttribute : CategoryAttribute
{
    public UnitTestAttribute() : base("unit-test") { }
}
