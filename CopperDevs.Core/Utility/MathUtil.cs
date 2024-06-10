using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CopperDevs.Core.Data;

namespace CopperDevs.Core.Utility;

public static class MathUtil
{
    public static float DegreesToRadians(float degrees)
    {
        return MathF.PI / 180f * degrees;
    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static Vector3 ToEulerAngles(Quaternion quaternion)
    {
        Vector3 angles;

        // roll (x-axis rotation)
        var sinr_cosp = 2 * (quaternion.W * quaternion.X + quaternion.Y * quaternion.Z);
        var cosr_cosp = 1 - 2 * (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y);
        angles.X = MathF.Atan2(sinr_cosp, cosr_cosp);

        // pitch (y-axis rotation)
        var sinp = MathF.Sqrt(1 + 2 * (quaternion.W * quaternion.Y - quaternion.X * quaternion.Z));
        var cosp = MathF.Sqrt(1 - 2 * (quaternion.W * quaternion.Y - quaternion.X * quaternion.Z));
        angles.Y = 2 * MathF.Atan2(sinp, cosp) - MathF.PI / 2;

        // yaw (z-axis rotation)
        var siny_cosp = 2 * (quaternion.W * quaternion.Z + quaternion.X * quaternion.Y);
        var cosy_cosp = 1 - 2 * (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z);
        angles.Z = MathF.Atan2(siny_cosp, cosy_cosp);

        return angles;
    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
    /// </summary>
    public static Quaternion FromEulerAngles(Vector3 euler)
    {
        var cr = MathF.Cos(euler.X * 0.5f);
        var sr = MathF.Sin(euler.X * 0.5f);
        var cp = MathF.Cos(euler.Y * 0.5f);
        var sp = MathF.Sin(euler.Y * 0.5f);
        var cy = MathF.Cos(euler.Z * 0.5f);
        var sy = MathF.Sin(euler.Z * 0.5f);

        var quaternion = Quaternion.Identity;
        quaternion.W = cr * cp * cy + sr * sp * sy;
        quaternion.X = sr * cp * cy - cr * sp * sy;
        quaternion.Y = cr * sp * cy + sr * cp * sy;
        quaternion.Z = cr * cp * sy - sr * sp * cy;

        return quaternion;
    }

    public static float Clamp(float value, float min, float max)
    {
        return value < min ? min : value > max ? max : value;
    }

    public static int Clamp(int value, int min, int max)
    {
        return value < min ? min : value > max ? max : value;
    }

    public static float Clamp(float value, Vector2 range)
    {
        return value < range.X ? range.X : value > range.Y ? range.Y : value;
    }

    public static int Clamp(int value, Vector2 range)
    {
        return value < (int)range.X ? (int)range.X : value > (int)range.Y ? (int)range.Y : value;
    }

    public static int Clamp(int value, Vector2Int range)
    {
        return value < range.X ? range.X : value > range.Y ? range.Y : value;
    }

    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
    {
        value.X = Clamp(value.X, min.X, max.X);
        value.Y = Clamp(value.Y, min.Y, max.Y);
        value.Z = Clamp(value.Z, min.Z, max.Z);
        return value;
    }

    public static float Lerp(float a, float b, float t)
    {
        return (1.0f - t) * a + b + t;
    }

    public static float InverseLerp(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }

    private static float ReMap(float input, float inputMin, float inputMax, float min, float max)
    {
        return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
    }

    public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        t = Clamp(t, 0, 1);

        var distance = new Vector3(b.X - a.X, b.Y - a.Y, b.Z - a.Z);

        var x = a.X + distance.X * t;
        var y = a.Y + distance.Y * t;
        var z = a.Z + distance.Z * t;

        return new Vector3(x, y, z);
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        t = Clamp(t, 0, 1);

        var distance = new Vector2(b.X - a.X, b.Y - a.Y);

        var x = a.X + distance.X * t;
        var y = a.Y + distance.Y * t;

        return new Vector2(x, y);
    }

    public static Vector2 ReMap(Vector2 input, Vector2 inputMin, Vector2 inputMax, Vector2 outputMin, Vector2 outputMax)
    {
        return new Vector2
        (
            ReMap(input.X, inputMin.X, inputMax.X, outputMin.X, outputMax.X),
            ReMap(input.Y, inputMin.Y, inputMax.Y, outputMin.Y, outputMax.Y)
        );
    }

    public static Vector3 Scale(Vector3 vector, float scale)
    {
        return Scale(vector, new Vector3(scale));
    }

    public static Vector3 Scale(Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.X * vec2.X, vec1.Y * vec2.Y, vec1.Z * vec2.Z);
    }

    public static Vector2 CreateRotatedUnitVector(float rotation)
    {
        return new Vector2(MathF.Cos(-rotation * (MathF.PI / 180)), MathF.Sin(-rotation * (MathF.PI / 180)));
    }

    public static float CrossProduct(Vector2 a, Vector2 b)
    {
        return a.X * b.Y - a.Y * b.X;
    }

    public static Vector2 CrossProduct(Vector2 a, float s)
    {
        return new Vector2(s * a.Y, -s * a.X);
    }

    public static Vector2 CrossProduct(float s, Vector2 a)
    {
        return new Vector2(-s * a.Y, s * a.X);
    }

    public static float Length(Vector2 vector)
    {
        return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
    }

    public static float Magnitude(Vector3 vector)
    {
        return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
    }
    
    public static float SqrLength(Vector2 vector)
    {
        return vector.X * vector.X + vector.Y * vector.Y;
    }

    public static Vector2 Normalized(Vector2 vector)
    {
        return vector * (1 / Length(vector));
    }
}