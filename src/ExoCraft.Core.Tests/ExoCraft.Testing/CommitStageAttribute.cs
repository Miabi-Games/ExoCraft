using NUnit.Framework;

namespace ExoCraft.Testing;

public class CommitStageAttribute : CategoryAttribute
{
    public CommitStageAttribute() : base("commit-stage") { }
}
