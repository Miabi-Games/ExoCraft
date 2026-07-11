using Godot;

namespace ExoCraft.Framework.GameSystems;

[GlobalClass]
public partial class GameSystemRootNode : GameSystemGroupNode
{
    [Export]
    public bool IsInitialized
    {
        get => _isInitialized;
        set { }
    }
    private bool _isInitialized = false;

    public override void Initialize()
    {
        if (!IsInitialized)
        {
            base.Initialize();
            _isInitialized = true;
        }
    }

    public override void Shutdown()
    {
        if (IsInitialized)
        {
            _isInitialized = false;
            base.Shutdown();
        }
    }

    public override void Update(double delta)
    {
        if (_isInitialized) base.Update(delta);
    }

    public override void Render(double delta)
    {
        if (_isInitialized) base.Render(delta);
    }
}
