#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CopperDevs.Core.Mathematics;

public struct Vector2 : IEquatable<Vector2>
{
    public float X;
    public float Y;

    public static Vector2 Zero => default;
    public static Vector2 One => new(1);
    public static Vector2 UnitX => new(1, 0);
    public static Vector2 UnitY => new(0, 1);

    public Vector2() : this(0)
    {
    }

    public Vector2(float value) : this(value, value)
    {
    }

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Vector2 other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
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
        return obj is Vector2 vector2 && Equals(vector2);
    }

    public static bool operator ==(Vector2 left, Vector2 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector2 left, Vector2 right)
    {
        return !(left == right);
    }

    public static Vector2 operator +(Vector2 left, Vector2 right)
    {
        return new Vector2(left.X + right.X, left.Y + right.Y);
    }

    public static Vector2 operator /(Vector2 left, Vector2 right)
    {
        return new Vector2(left.X / right.X, left.Y / right.Y);
    }

    public static Vector2 operator /(Vector2 value1, float value2)
    {
        return value1 / new Vector2(value2);
    }

    public static Vector2 operator *(Vector2 left, Vector2 right)
    {
        return new Vector2(left.X * right.X, left.Y * right.Y);
    }

    public static Vector2 operator *(Vector2 left, float right)
    {
        return left * new Vector2(right);
    }

    public static Vector2 operator *(int left, Vector2 right)
    {
        return right * left;
    }

    public static Vector2 operator -(Vector2 left, Vector2 right)
    {
        return new Vector2(left.X - right.X, left.Y - right.Y);
    }

    public static implicit operator Vector2(Vector2Int value)
    {
        return new Vector2(value.X, value.Y);
    }

    public static implicit operator Vector2Int(Vector2 value)
    {
        return new Vector2Int((int)value.X, (int)value.Y);
    }

    public static implicit operator SystemVector2(Vector2 value)
    {
        return new SystemVector2(value.X, value.Y);
    }

    public static implicit operator Vector2(SystemVector2 value)
    {
        return new Vector2((int)value.X, (int)value.Y);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}