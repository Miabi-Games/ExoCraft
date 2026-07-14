using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class CameraLifecycleSystemNode
    : GameSystemHostNode<CameraLifecycleSystem>
{
    protected override CameraLifecycleSystem CreateHostedSystem()
    {
        return new(GameSession.Instance.SimWorld);
    }
}
