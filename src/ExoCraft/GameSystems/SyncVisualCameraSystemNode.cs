using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class SyncVisualCameraSystemNode
    : GameSystemHostNode<SyncVisualCameraSystem>
{
    protected override SyncVisualCameraSystem CreateHostedSystem()
    {
        var gameSession = GameSession.Instance;

        return new(
            gameSession.SimWorld,
            gameSession.VisualWorld);
    }
}
