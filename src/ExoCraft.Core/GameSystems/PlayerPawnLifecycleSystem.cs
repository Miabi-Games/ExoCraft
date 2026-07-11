using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.VisualWorld;

namespace ExoCraft.GameSystems;

public class PlayerPawnLifecycleSystem : GameSystem
{
    public PlayerPawnLifecycleSystem(IVisualWorld visualWorld)
    {
        _visualWorld = visualWorld;
    }

    public override void Initialize()
    {
        _playerPawn = _visualWorld.CreatePlayerPawn();
    }

    public override void Shutdown()
    {
        _playerPawn?.Free();
    }

    private readonly IVisualWorld _visualWorld;
    private IVisualPawn? _playerPawn;
}
