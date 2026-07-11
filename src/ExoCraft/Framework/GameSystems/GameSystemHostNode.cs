using Godot;

using System.Diagnostics.CodeAnalysis;

namespace ExoCraft.Framework.GameSystems;

public abstract partial class GameSystemHostNode<TSystem> : GameSystemNode
    where TSystem : GameSystem
{
    public TSystem System { get; private set; } = null!;

    [Export]
    public bool IsInitialized
    {
        get => _isInitialized;
        set { }
    }
    private bool _isInitialized = false;

    public sealed override void CreateSystem()
    {
        System = CreateHostedSystem();
    }

    public override void Initialize()
    {
        if (System is not null)
        {
            System.Initialize();
            _isInitialized = true;
        }
    }

    public override void Shutdown()
    {
        if (_isInitialized)
        {
            _isInitialized = false;
            System?.Shutdown();
        }
    }

    public override void Update(double delta)
    {
        if (_isInitialized) System?.Update(delta);
    }

    public override void Render(double delta)
    {
        if (_isInitialized) System?.Render(delta);
    }

    protected abstract TSystem CreateHostedSystem();
}
