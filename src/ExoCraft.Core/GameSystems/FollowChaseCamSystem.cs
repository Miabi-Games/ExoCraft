using DefaultEcs;

using ExoCraft.EntityComponents;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.Math;
using ExoCraft.Framework.SimWorlds;

namespace ExoCraft.GameSystems;

public class FollowChaseCamSystem : GameSystem
{
    public FollowChaseCamSystem(ISimWorld simWorld)
    {
        _ecsWorld = simWorld.EcsWorld;
    }

    public override void Initialize()
    {
        _chaseCams = _ecsWorld.GetEntities()
            .With<Camera>()
            .With<ChaseCam>()
            .AsSet();
    }

    public override void Shutdown()
    {
        _chaseCams.Dispose();
    }

    public override void Update(double delta)
    {
        foreach (Entity entity in _chaseCams.GetEntities())
        {
            ref ChaseCam chaseCam = ref entity.Get<ChaseCam>();
            Entity target = chaseCam.Target;

            if (!target.IsAlive ||
                !target.Has<Pawn>() ||
                !target.Has<CameraLookAt>())
            {
                continue;
            }

            ref Camera camera = ref entity.Get<Camera>();
            ref Pawn pawn = ref target.Get<Pawn>();
            ref CameraLookAt lookAt = ref target.Get<CameraLookAt>();

            double3 targetPosition =
                pawn.Transform.transform_position(lookAt.Position);
            double3basis cameraRotation = pawn.Transform.rotation;
            cameraRotation.rotate_local_x(chaseCam.Pitch);

            camera.Transform.position = targetPosition +
                cameraRotation.transform_vector(
                    chaseCam.Distance * double3.unit_z);
            camera.Transform.rotation = cameraRotation;
        }
    }

    private readonly World _ecsWorld;

    private EntitySet _chaseCams = null!;
}
