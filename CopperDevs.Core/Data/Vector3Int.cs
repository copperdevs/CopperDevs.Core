using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CopperDevs.Core.Data;

public struct Vector3Int(int x, int y, int z) : IEquatable<Vector3Int>, IFormattable
{
    public int X = x;
    public int Y = y;
    public int Z = z;

    public static Vector3Int Zero => default;
    public static Vector3Int One => new(1);


    public Vector3Int() : this(0, 0, 0) { }

    public Vector3Int(int value) : this(value, value, value)
    {
    }

    public readonly bool Equals(Vector3Int other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public readonly override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        return $"<{X.ToString(format, formatProvider)}{separator} {Y.ToString(format, formatProvider)}>";
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector3Int vector3Int && Equals(vector3Int);
    }

    public static bool operator ==(Vector3Int left, Vector3Int right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector3Int left, Vector3Int right)
    {
        return !(left == right);
    }

    public static Vector3Int operator +(Vector3Int left, Vector3Int right)
    {
        return new Vector3Int(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vector3Int operator /(Vector3Int left, Vector3Int right)
    {
        return new Vector3Int(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    public static Vector3Int operator /(Vector3Int value1, int value2)
    {
        return value1 / new Vector3Int(value2);
    }

    public static Vector3Int operator *(Vector3Int left, Vector3Int right)
    {
        return new Vector3Int(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static Vector3Int operator *(Vector3Int left, int right)
    {
        return left * new Vector3Int(right);
    }

    public static Vector3Int operator *(int left, Vector3Int right)
    {
        return right * left;
    }

    public static Vector3Int operator -(Vector3Int left, Vector3Int right)
    {
        return new Vector3Int(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static implicit operator SystemVector3(Vector3Int value)
    {
        return new SystemVector3(value.X, value.Y, value.Z);
    }

    public static implicit operator Vector3Int(SystemVector3 value)
    {
        return new Vector3Int((int)value.X, (int)value.Y, (int)value.Z);
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}
