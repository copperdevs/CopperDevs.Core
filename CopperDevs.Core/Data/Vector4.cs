#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CopperDevs.Core.Data;

public struct Vector4 : IEquatable<Vector4>
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public static Vector3 Zero => default;
    public static Vector3 One => new(1);

    public Vector4() : this(0)
    {
    }

    public Vector4(float value) : this(value, value, value, value)
    {
    }

    public Vector4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public bool Equals(Vector4 other)
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

    public override bool Equals(object? obj)
    {
        return obj is Vector4 vector4 && Equals(vector4);
    }

    public static bool operator ==(Vector4 left, Vector4 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector4 left, Vector4 right)
    {
        return !(left == right);
    }

    public static Vector4 operator +(Vector4 left, Vector4 right)
    {
        return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    public static Vector4 operator /(Vector4 left, Vector4 right)
    {
        return new Vector4(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
    }

    public static Vector4 operator /(Vector4 value1, float value2)
    {
        return value1 / new Vector4(value2);
    }

    public static Vector4 operator *(Vector4 left, Vector4 right)
    {
        return new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }

    public static Vector4 operator *(Vector4 left, float right)
    {
        return left * new Vector4(right);
    }

    public static Vector4 operator *(float left, Vector4 right)
    {
        return right * left;
    }

    public static Vector4 operator -(Vector4 left, Vector4 right)
    {
        return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }

    public static implicit operator Vector4(Vector4Int value)
    {
        return new Vector4(value.X, value.Y, value.Z, value.W);
    }

    public static implicit operator Vector4Int(Vector4 value)
    {
        return new Vector4Int((int)value.X, (int)value.Y, (int)value.Z, (int)value.W);
    }

    public static implicit operator SystemVector4(Vector4 value)
    {
        return new SystemVector4(value.X, value.Y, value.Z, value.W);
    }

    public static implicit operator Vector4(SystemVector4 value)
    {
        return new Vector4((int)value.X, (int)value.Y, (int)value.Z, (int)value.W);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }
}