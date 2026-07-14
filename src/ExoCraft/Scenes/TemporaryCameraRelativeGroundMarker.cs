using ExoCraft.Framework.Math;

using Godot;

namespace ExoCraft.Scenes;

// This is a temporary presentation shim for the prototype ground marker.
// Remove this script when the marker is replaced by simulation-backed terrain.
public partial class TemporaryCameraRelativeGroundMarker : MeshInstance3D
{
    public override void _Ready()
    {
        _visualWorld = (VisualWorld)GetParent();
        _simulationPosition = ToDouble3(Position);

        ApplyCameraRelativePosition();
    }

    public override void _Process(double delta)
    {
        ApplyCameraRelativePosition();
    }

    private void ApplyCameraRelativePosition()
    {
        Position = ToVector3(
            _simulationPosition - _visualWorld.CameraTransform.position);
    }

    private static double3 ToDouble3(Vector3 value)
        => new(value.X, value.Y, value.Z);

    private static Vector3 ToVector3(double3 value)
        => new((float)value.x, (float)value.y, (float)value.z);

    private VisualWorld _visualWorld = null!;
    private double3 _simulationPosition;
}
