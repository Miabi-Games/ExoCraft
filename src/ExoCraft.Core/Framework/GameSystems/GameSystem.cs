namespace ExoCraft.Framework.GameSystems;

public class GameSystem
{
    public virtual void Initialize() { }
    public virtual void Shutdown() { }

    public virtual void Update(double delta) { }
    public virtual void Render(double delta) { }
}
