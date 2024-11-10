#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CopperDevs.Core.Mathematics;

public struct Vector3(float x, float y, float z) : IEquatable<Vector3>
{
    public float X = x;
    public float Y = y;
    public float Z = z;

    public static Vector3 Zero => default;
    public static Vector3 One => new(1);

    public static Vector3 UnitX => new(1.0f, 0.0f, 0.0f);

    public static Vector3 UnitY => new(0.0f, 1.0f, 0.0f);

    public static Vector3 UnitZ => new(0.0f, 0.0f, 1.0f);

    public Vector3() : this(0)
    {
    }

    public Vector3(float value) : this(value, value, value)
    {
    }

    public Vector3(Vector2 xy) : this(xy.X, xy.Y, 0)
    {
    }

    public bool Equals(Vector3 other)
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
        return obj is Vector3 vector3 && Equals(vector3);
    }

    public static bool operator ==(Vector3 left, Vector3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector3 left, Vector3 right)
    {
        return !(left == right);
    }

    public static Vector3 operator +(Vector3 left, Vector3 right)
    {
        return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vector3 operator /(Vector3 left, Vector3 right)
    {
        return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    public static Vector3 operator /(Vector3 value1, float value2)
    {
        return value1 / new Vector3(value2);
    }

    public static Vector3 operator *(Vector3 left, Vector3 right)
    {
        return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static Vector3 operator *(Vector3 left, float right)
    {
        return left * new Vector3(right);
    }

    public static Vector3 operator *(float left, Vector3 right)
    {
        return right * left;
    }

    public static Vector3 operator -(Vector3 left, Vector3 right)
    {
        return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static implicit operator Vector3(Vector3Int value)
    {
        return new Vector3(value.X, value.Y, value.Z);
    }

    public static implicit operator Vector3Int(Vector3 value)
    {
        return new Vector3Int((int)value.X, (int)value.Y, (int)value.Z);
    }

    public static implicit operator SystemVector3(Vector3 value)
    {
        return new SystemVector3(value.X, value.Y, value.Z);
    }

    public static implicit operator Vector3(SystemVector3 value)
    {
        return new Vector3((int)value.X, (int)value.Y, (int)value.Z);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}