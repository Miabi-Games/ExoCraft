using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class UpdateVoxelChunkMeshesSystemNode
    : GameSystemHostNode<UpdateVoxelChunkMeshesSystem>
{
    protected override UpdateVoxelChunkMeshesSystem CreateHostedSystem()
    {
        return new(GameSession.Instance.SimWorld);
    }
}
