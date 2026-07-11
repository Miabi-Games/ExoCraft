using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.GameSystems;

[GlobalClass]
public partial class PlayerPawnLifecycleSystemNode
    : GameSystemHostNode<PlayerPawnLifecycleSystem>
{
    public override void Initialize()
    {
        GD.Print("Initialized");
    }

    public override void Shutdown()
    {
        GD.Print("Shutdown");
    }

    public override void Update(double delta)
    {
        _updateCount++;

        if (_updateCount <= 2) GD.Print($"Update {_updateCount}");
        else if (_updateCount == 3) GD.Print($"More updates...");
    }
    private int _updateCount = 0;

    protected override PlayerPawnLifecycleSystem CreateHostedSystem()
    {
        return new();
    }
}
