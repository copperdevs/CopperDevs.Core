using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CopperDevs.Core.Data;

public struct Vector4Int(int x, int y, int z, int w) : IEquatable<Vector4Int>
{
    public int X = x;
    public int Y = y;
    public int Z = z;
    public int W = w;

    public static Vector3Int Zero => default;
    public static Vector3Int One => new(1);


    public Vector4Int() : this(0, 0, 0, 0) { }

    public Vector4Int(int value) : this(value, value, value, value)
    {
    }

    public readonly bool Equals(Vector4Int other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
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

    public readonly override bool Equals(object? obj)
    {
        return obj is Vector3Int vector3Int && Equals(vector3Int);
    }

    public static bool operator ==(Vector4Int left, Vector4Int right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector4Int left, Vector4Int right)
    {
        return !(left == right);
    }

    public static Vector4Int operator +(Vector4Int left, Vector4Int right)
    {
        return new Vector4Int(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    public static Vector4Int operator /(Vector4Int left, Vector4Int right)
    {
        return new Vector4Int(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
    }

    public static Vector4Int operator /(Vector4Int value1, int value2)
    {
        return value1 / new Vector4Int(value2);
    }

    public static Vector4Int operator *(Vector4Int left, Vector4Int right)
    {
        return new Vector4Int(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }

    public static Vector4Int operator *(Vector4Int left, int right)
    {
        return left * new Vector4Int(right);
    }

    public static Vector4Int operator *(int left, Vector4Int right)
    {
        return right * left;
    }

    public static Vector4Int operator -(Vector4Int left, Vector4Int right)
    {
        return new Vector4Int(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }

    public static implicit operator SystemVector4(Vector4Int value)
    {
        return new SystemVector4(value.X, value.Y, value.Z, value.W);
    }

    public static implicit operator Vector4Int(SystemVector4 value)
    {
        return new Vector4Int((int)value.X, (int)value.Y, (int)value.Z, (int)value.W);
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }
}
