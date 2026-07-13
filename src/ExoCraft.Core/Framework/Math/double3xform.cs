using System.Diagnostics.CodeAnalysis;

namespace ExoCraft.Framework.Math;

[SuppressMessage("Style", "IDE1006:Naming Styles")]
public record struct double3xform
{
    // ─────────────────────────────────────────────────────────────────────────

    public double3 position;
    public double3basis rotation;
    public double scale;

    // ─────────────────────────────────────────────────────────────────────────

    public readonly static double3xform identity = new(double3.zero, double3basis.identity, 1.0);

    // ─────────────────────────────────────────────────────────────────────────

    public double3xform(double3 position, double3basis rotation, double scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    // ─────────────────────────────────────────────────────────────────────────
}
