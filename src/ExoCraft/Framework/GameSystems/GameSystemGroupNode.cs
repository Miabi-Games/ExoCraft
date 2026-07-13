using Godot;

namespace ExoCraft.Framework.GameSystems;

[GlobalClass]
public partial class GameSystemGroupNode : GameSystemNode
{
    public override void CreateSystem()
    {
        foreach (var child in GetChildren())
        {
            if (child is GameSystemNode system) system.CreateSystem();
        }
    }

    public override void Initialize()
    {
        foreach (var child in GetChildren())
        {
            if (child is GameSystemNode system) system.Initialize();
        }
    }

    public override void Shutdown()
    {
        var children = GetChildren();
        children.Reverse();

        foreach (var child in children)
        {
            if (child is GameSystemNode system) system.Shutdown();
        }
    }

    public override void Update(double delta)
    {
        foreach (var child in GetChildren())
        {
            if (child is GameSystemNode system) system.Update(delta);
        }
    }

    public override void Render(double delta)
    {
        foreach (var child in GetChildren())
        {
            if (child is GameSystemNode system) system.Render(delta);
        }
    }
}
