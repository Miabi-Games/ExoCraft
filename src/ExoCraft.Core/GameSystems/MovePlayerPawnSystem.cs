using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.InputProviders;
using ExoCraft.Framework.Math;
using ExoCraft.Framework.SimWorlds;

namespace ExoCraft.GameSystems;

public class MovePlayerPawnSystem : GameSystem
{
    public MovePlayerPawnSystem(
        ISimWorld simWorld,
        IInputProvider inputProvider)
    {
        _ecsWorld = simWorld.EcsWorld;
        _inputProvider = inputProvider;
    }

    public override void Initialize()
    {
        _playerPawns = _ecsWorld.GetEntities()
            .With<PlayerPawn>()
            .With<Pawn>()
            .AsSet();
    }

    public override void Shutdown()
    {
        _playerPawns.Dispose();
    }

    public override void Update(double delta)
    {
        float3 movementInput = _inputProvider.MovementInput;
        float3 rotationInput = _inputProvider.RotationInput;

        foreach (var entity in _playerPawns.GetEntities())
        {
            ref var pawn = ref entity.Get<Pawn>();

            MovePawn(ref pawn, movementInput, delta);
            RotatePawn(ref pawn, rotationInput, delta);
        }
    }

    private static void MovePawn(
        ref Pawn pawn,
        float3 movementInput,
        double delta)
    {
        double distance = MovementSpeed * delta;
        double3basis rotation = pawn.Transform.rotation;
        double3 movement =
            movementInput.x * rotation.x +
            movementInput.z * rotation.z;

        pawn.Transform.position += distance * movement;
    }

    private static void RotatePawn(
        ref Pawn pawn,
        float3 rotationInput,
        double delta)
    {
        double angle = RotationSpeed * delta;

        pawn.Transform.rotation.rotate_parent_y(rotationInput.y * angle);
    }

    private const double MovementSpeed = 5.0;
    private const double RotationSpeed = double.Pi * 2 / 3;

    private readonly World _ecsWorld;
    private readonly IInputProvider _inputProvider;

    private EntitySet _playerPawns = null!;
}
