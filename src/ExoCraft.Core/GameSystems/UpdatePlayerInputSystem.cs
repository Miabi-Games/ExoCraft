using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.InputProviders;
using ExoCraft.Framework.SimWorlds;

namespace ExoCraft.GameSystems;

public class UpdatePlayerInputSystem : GameSystem
{
    public UpdatePlayerInputSystem(
        ISimWorld simWorld,
        IInputProvider inputProvider)
    {
        _ecsWorld = simWorld.EcsWorld;
        _inputProvider = inputProvider;
    }

    public override void Initialize()
    {
        _ecsWorld.Set<PlayerInput>();
    }

    public override void Shutdown()
    {
        _ecsWorld.Remove<PlayerInput>();
    }

    public override void Update(double delta)
    {
        ref var playerInput = ref _ecsWorld.Get<PlayerInput>();

        playerInput.Movement = _inputProvider.MovementInput;
        playerInput.Rotation = _inputProvider.RotationInput;
    }

    private readonly World _ecsWorld;
    private readonly IInputProvider _inputProvider;
}
