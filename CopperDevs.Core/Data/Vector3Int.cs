using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace CopperDevs.Core.Data;

public struct Vector3Int : IEquatable<Vector3Int>, IFormattable
{
    public int X;
    public int Y;
    public int Z;

    public static Vector3Int Zero => default;
    public static Vector3Int One => new(1);

    public Vector3Int(int value) : this(value, value, value)
    {
    }

    public Vector3Int(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(Vector3Int other)
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
        return obj is Vector2Int vector2Int && Equals(vector2Int);
    }

    public static bool operator ==(Vector3Int left, Vector3Int right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector3Int left, Vector3Int right)
    {
        return !(left == right);
    }

    public static Vector2Int operator +(Vector3Int left, Vector3Int right)
    {
        return new Vector2Int(left.X + right.X, left.Y + right.Y);
    }

    public static Vector2Int operator /(Vector3Int left, Vector3Int right)
    {
        return new Vector2Int(left.X / right.X, left.Y / right.Y);
    }

    public static Vector2Int operator /(Vector3Int value1, int value2)
    {
        return value1 / new Vector3Int(value2);
    }

    public static Vector2Int operator *(Vector3Int left, Vector3Int right)
    {
        return new Vector2Int(left.X * right.X, left.Y * right.Y);
    }

    public static Vector2Int operator *(Vector3Int left, int right)
    {
        return left * new Vector3Int(right);
    }

    public static Vector2Int operator *(int left, Vector3Int right)
    {
        return right * left;
    }

    public static Vector2Int operator -(Vector3Int left, Vector3Int right)
    {
        return new Vector2Int(left.X - right.X, left.Y - right.Y);
    }

    public static implicit operator Vector3(Vector3Int value)
    {
        return new Vector3(value.X, value.Y, value.Z);
    }

    public static implicit operator Vector3Int(Vector3 value)
    {
        return new Vector3Int((int)value.X, (int)value.Y, (int)value.Z);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}