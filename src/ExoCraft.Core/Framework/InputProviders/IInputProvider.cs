using ExoCraft.Framework.Math;

namespace ExoCraft.Framework.InputProviders;

public interface IInputProvider
{
    /// <summary>
    /// Gets movement intent in pawn-local x, y, and z coordinates.
    /// </summary>
    float3 MovementInput { get; }

    /// <summary>
    /// Gets rotation intent as yaw on y. The other components are reserved for
    /// future rotation support.
    /// </summary>
    float3 RotationInput { get; }
}
