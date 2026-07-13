using ExoCraft.Framework.Math;

using Godot;

namespace ExoCraft.Framework.InputProviders;

[GlobalClass]
public partial class InputProvider : Node, IInputProvider
{
    [Export]
    public float MouseSensitivity = 0.001f;

    [Export]
    public bool InvertPitch = true;

    public float3 MovementInput { get; private set; }
    public float3 RotationInput { get; private set; }

    public override void _Process(double delta)
    {
        MovementInput = (
            Godot.Input.GetAxis("move_left", "move_right"),
            Godot.Input.GetAxis("move_down", "move_up"),
            Godot.Input.GetAxis("move_forward", "move_backward"));

        Vector2 mouseVelocity = Godot.Input.GetLastMouseVelocity();
        float pitchDirection = InvertPitch ? 1.0f : -1.0f;

        RotationInput = (
            pitchDirection * mouseVelocity.Y * MouseSensitivity,
            -mouseVelocity.X * MouseSensitivity,
            0.0f);
    }
}
