using Godot;

namespace ExoCraft.Framework;

[GlobalClass]
public abstract partial class GameSystemNode : Node
{
    public virtual void CreateSystem() { }

    public virtual void Initialize() { }
    public virtual void Shutdown() { }

    public virtual void Update(double delta) { }
    public virtual void Render(double delta) { }
}
