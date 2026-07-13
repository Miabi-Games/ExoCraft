using System.Diagnostics.CodeAnalysis;

namespace ExoCraft.Framework.Math;

[SuppressMessage("Style", "IDE1006:Naming Styles")]
public record struct double3basis
{
    // ─────────────────────────────────────────────────────────────────────────
    // This is a column major 3x3 column basis matrix. Column major refers to
    // its storage. Each basis vector is stored contiguously, but it represents
    // a column, not a row. So the storage order is e11, e21, e31, e12, e22,
    // e32, e13, e23, e33 in standard matrix notation (e.g., e23 is the element
    // in the third column of the second row).
    //
    // This type is only intended for use as a pure rotation matrix, usually to
    // represent an entity's orientation relative to its parent (which is what
    // makes keeping the basis vectors easily accessible valuable).
    //
    // As such, this type's inverse operators are only intended to support pure
    // rotation matrices, but they incidentally also support reflections. The
    // inverse operators treat the transpose of the matrix as its inverse, which
    // is only true when the basis vectors are orthonormal (they are unit length
    // and each is perpendicular to each of the other two). Both rotation and
    // reflection preserve the unit length and orthogonality of the basis
    // vectors. Hence, reflections are also supported by the inverse operators,
    // but transformations that do not preserve these properties are not.
    //
    // Note: Operators (including inverse operators) to be added later.
    // ─────────────────────────────────────────────────────────────────────────

    // ─────────────────────────────────────────────────────────────────────────

    public double3 x, y, z;

    public double e11 { readonly get => x.x; set => x.x = value; }
    public double e12 { readonly get => y.x; set => y.x = value; }
    public double e13 { readonly get => z.x; set => z.x = value; }

    public double e21 { readonly get => x.y; set => x.y = value; }
    public double e22 { readonly get => y.y; set => y.y = value; }
    public double e23 { readonly get => z.y; set => z.y = value; }

    public double e31 { readonly get => x.z; set => x.z = value; }
    public double e32 { readonly get => y.z; set => y.z = value; }
    public double e33 { readonly get => z.z; set => z.z = value; }

    // ─────────────────────────────────────────────────────────────────────────

    public static readonly double3basis zero = (double3.zero, double3.zero, double3.zero);
    public static readonly double3basis identity = (double3.unit_x, double3.unit_y, double3.unit_z);

    // ─────────────────────────────────────────────────────────────────────────

    public double3basis(double3 x, double3 y, double3 z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public double3basis(
        double e11, double e12, double e13,
        double e21, double e22, double e23,
        double e31, double e32, double e33)
    {
        x = new(e11, e21, e31);
        y = new(e12, e22, e32);
        z = new(e13, e23, e33);
    }

    public void Deconstruct(out double3 x, out double3 y, out double3 z)
    {
        x = this.x; y = this.y; z = this.z;
    }

    public static implicit operator double3basis(in (double3 x, double3 y, double3 z) value)
        => new(value.x, value.y, value.z);

    /// <summary>
    /// Rotates this basis around its local x-axis by an angle in radians.
    /// </summary>
    public void rotate_local_x(double angle)
    {
        (double sin, double cos) = System.Math.SinCos(angle);

        double3 original_y = y;
        y = cos * y + sin * z;
        z = -sin * original_y + cos * z;
    }

    /// <summary>
    /// Rotates this basis around its local y-axis by an angle in radians.
    /// </summary>
    public void rotate_local_y(double angle)
    {
        (double sin, double cos) = System.Math.SinCos(angle);

        double3 original_x = x;
        x = cos * x - sin * z;
        z = sin * original_x + cos * z;
    }

    /// <summary>
    /// Rotates this basis around its parent-space y-axis by an angle in radians.
    /// </summary>
    public void rotate_parent_y(double angle)
    {
        (double sin, double cos) = System.Math.SinCos(angle);

        double original_x_x = x.x;
        x.x = cos * x.x + sin * x.z;
        x.z = -sin * original_x_x + cos * x.z;

        double original_y_x = y.x;
        y.x = cos * y.x + sin * y.z;
        y.z = -sin * original_y_x + cos * y.z;

        double original_z_x = z.x;
        z.x = cos * z.x + sin * z.z;
        z.z = -sin * original_z_x + cos * z.z;
    }

    // ─────────────────────────────────────────────────────────────────────────
}
