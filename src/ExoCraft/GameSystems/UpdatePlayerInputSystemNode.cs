using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class UpdatePlayerInputSystemNode
    : GameSystemHostNode<UpdatePlayerInputSystem>
{
    protected override UpdatePlayerInputSystem CreateHostedSystem()
    {
        var gameSession = GameSession.Instance;

        return new(
            gameSession.SimWorld,
            gameSession.InputProvider);
    }
}
