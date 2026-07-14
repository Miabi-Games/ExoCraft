using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class VoxelChunkLifecycleSystemNode
    : GameSystemHostNode<VoxelChunkLifecycleSystem>
{
    protected override VoxelChunkLifecycleSystem CreateHostedSystem()
    {
        var gameSession = GameSession.Instance;

        return new(
            gameSession.SimWorld,
            gameSession.VisualWorld);
    }
}
