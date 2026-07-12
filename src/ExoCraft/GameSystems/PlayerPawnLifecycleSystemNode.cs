using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class PlayerPawnLifecycleSystemNode
    : GameSystemHostNode<PlayerPawnLifecycleSystem>
{
    protected override PlayerPawnLifecycleSystem CreateHostedSystem()
    {
        var gameSession = GameSession.Instance;

        return new(
            gameSession.SimWorld,
            gameSession.VisualWorld);
    }
}
