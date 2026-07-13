using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class MovePlayerPawnSystemNode
    : GameSystemHostNode<MovePlayerPawnSystem>
{
    protected override MovePlayerPawnSystem CreateHostedSystem()
    {
        var gameSession = GameSession.Instance;

        return new(
            gameSession.SimWorld,
            gameSession.InputProvider);
    }
}
