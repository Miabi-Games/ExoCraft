using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class SyncVisualPawnsSystemNode
    : GameSystemHostNode<SyncVisualPawnsSystem>
{
    protected override SyncVisualPawnsSystem CreateHostedSystem()
    {
        return new(GameSession.Instance.SimWorld);
    }
}
