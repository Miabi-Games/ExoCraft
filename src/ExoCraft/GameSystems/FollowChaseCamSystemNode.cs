using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class FollowChaseCamSystemNode
    : GameSystemHostNode<FollowChaseCamSystem>
{
    protected override FollowChaseCamSystem CreateHostedSystem()
    {
        return new(GameSession.Instance.SimWorld);
    }
}
