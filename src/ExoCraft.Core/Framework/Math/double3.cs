using System.Diagnostics.CodeAnalysis;

namespace ExoCraft.Framework.Math;

[SuppressMessage("Style", "IDE1006:Naming Styles")]
public record struct double3
{
    // ─────────────────────────────────────────────────────────────────────────

    public double x, y, z;

    // ─────────────────────────────────────────────────────────────────────────

    public static readonly double3 zero = (0, 0, 0);

    public static readonly double3 unit_x = (1, 0, 0);
    public static readonly double3 unit_y = (0, 1, 0);
    public static readonly double3 unit_z = (0, 0, 1);

    public static readonly double3 unit_nx = (-1, 0, 0);
    public static readonly double3 unit_ny = (0, -1, 0);
    public static readonly double3 unit_nz = (0, 0, -1);

    // ─────────────────────────────────────────────────────────────────────────
    // The following is the standard coordinate system from the perspective of a
    // viewer or camera. It is also the system used for movement and physics.
    // Models should be authored to face towards the viewer, and so must be
    // rotated 180 degrees around the y-axis to be facing forward (so they are
    // facing away from the camera, which is the direction that they should move
    // when moving forward). Remember: forward is away from the camera.
    // ─────────────────────────────────────────────────────────────────────────

    public static readonly double3 forward = (0, 0, -1);
    public static readonly double3 back = (0, 0, 1);

    public static readonly double3 left = (-1, 0, 0);
    public static readonly double3 right = (1, 0, 0);

    public static readonly double3 up = (0, 1, 0);
    public static readonly double3 down = (0, -1, 0);

    // ─────────────────────────────────────────────────────────────────────────

    public double3(double x, double y, double z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public readonly void Deconstruct(out double x, out double y, out double z)
    {
        x = this.x; y = this.y; z = this.z;
    }

    public static implicit operator double3(in (double x, double y, double z) tuple)
        => new(tuple.x, tuple.y, tuple.z);

    // ─────────────────────────────────────────────────────────────────────────

    public static double3 operator +(double3 v) => new(+v.x, +v.y, +v.z);
    public static double3 operator -(double3 v) => new(-v.x, -v.y, -v.z);

    public static double3 operator +(double3 lhs, double3 rhs) => new(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
    public static double3 operator -(double3 lhs, double3 rhs) => new(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);

    public static double3 operator *(double3 lhs, double rhs) => new(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
    public static double3 operator *(double lhs, double3 rhs) => new(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);

    // ─────────────────────────────────────────────────────────────────────────
}
