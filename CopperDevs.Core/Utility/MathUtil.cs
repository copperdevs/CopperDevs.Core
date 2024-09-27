using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CopperDevs.Core.Data;

namespace CopperDevs.Core.Utility;

/// <summary>
/// Common math methods not included in <see cref="MathF"/>
/// </summary>
public static class MathUtil
{
    /// <summary>
    /// Convert degrees to radians
    /// </summary>
    /// <param name="degrees">Degrees</param>
    /// <returns>The input degrees value in radians</returns>
    public static float DegreesToRadians(float degrees)
    {
        return MathF.PI / 180f * degrees;
    }
    
    /// <summary>
    /// Convert radians to degrees
    /// </summary>
    /// <param name="radians">Radians</param>
    /// <returns>The input radians value in degrees</returns>
    public static float RadiansToDegrees(float radians)
    {
        return radians * (180f / MathF.PI);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
    /// </remarks>
    public static SystemVector3 ToEulerAngles(Quaternion quaternion)
    {
        SystemVector3 angles;

        // roll (x-axis rotation)
        var sinrCosp = 2 * (quaternion.W * quaternion.X + quaternion.Y * quaternion.Z);
        var cosrCosp = 1 - 2 * (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y);
        angles.X = MathF.Atan2(sinrCosp, cosrCosp);

        // pitch (y-axis rotation)
        var sinp = 2 * (quaternion.W * quaternion.Y - quaternion.Z * quaternion.X);
        angles.Y = MathF.Abs(sinp) >= 1 ? MathF.CopySign(MathF.PI / 2, sinp) : MathF.Asin(sinp);

        // yaw (z-axis rotation)
        var sinyCosp = 2 * (quaternion.W * quaternion.Z + quaternion.X * quaternion.Y);
        var cosyCosp = 1 - 2 * (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z);
        angles.Z = MathF.Atan2(sinyCosp, cosyCosp);

        return angles;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
    /// </remarks>
    public static Quaternion FromEulerAngles(SystemVector3 euler)
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

    /// <summary>
    /// Clamp a value between a range
    /// </summary>
    /// <param name="value">Value to clamp</param>
    /// <param name="min">Minimum value of the clamp range</param>
    /// <param name="max">Maximum value of the clamp range</param>
    /// <returns>The clamped value</returns>
    public static float Clamp(float value, float min, float max)
    {
        return value < min ? min : value > max ? max : value;
    }


    /// <summary>
    /// Clamp a value between a range
    /// </summary>
    /// <param name="value">Value to clamp</param>
    /// <param name="min">Minimum value of the clamp range</param>
    /// <param name="max">Maximum value of the clamp range</param>
    /// <returns>The clamped value</returns>
    public static int Clamp(int value, int min, int max)
    {
        return value < min ? min : value > max ? max : value;
    }

    /// <summary>
    /// Clamp a value between a range
    /// </summary>
    /// <param name="value">Value to clamp</param>
    /// <param name="range">Range of the values to clamp</param>
    /// <returns>The clamped value</returns>
    public static float Clamp(float value, SystemVector2 range)
    {
        return value < range.X ? range.X : value > range.Y ? range.Y : value;
    }

    /// <summary>
    /// Clamp a value between a range
    /// </summary>
    /// <param name="value">Value to clamp</param>
    /// <param name="range">Range of the values to clamp</param>
    /// <returns>The clamped value</returns>
    public static int Clamp(int value, SystemVector2 range)
    {
        return value < (int)range.X ? (int)range.X : value > (int)range.Y ? (int)range.Y : value;
    }

    /// <summary>
    /// Clamp a value between a range
    /// </summary>
    /// <param name="value">Value to clamp</param>
    /// <param name="range">Range of the values to clamp</param>
    /// <returns>The clamped value</returns>
    public static int Clamp(int value, Vector2Int range)
    {
        return value < range.X ? range.X : value > range.Y ? range.Y : value;
    }

    /// <summary>
    /// Clamp a value between a range
    /// </summary>
    /// <param name="value">Value to clamp</param>
    /// <param name="min">Minimum value of the clamp range</param>
    /// <param name="max">Maximum value of the clamp range</param>
    /// <returns>The clamped value</returns>
    public static SystemVector3 Clamp(SystemVector3 value, SystemVector3 min, SystemVector3 max)
    {
        value.X = Clamp(value.X, min.X, max.X);
        value.Y = Clamp(value.Y, min.Y, max.Y);
        value.Z = Clamp(value.Z, min.Z, max.Z);
        return value;
    }

    /// <summary>
    /// Linearly interpolates between two points.
    /// </summary>
    /// <param name="a">Start value</param>
    /// <param name="b">End value</param>
    /// <param name="t">Value used to interpolate between a and b.</param>
    /// <returns>Interpolated value</returns>
    public static float Lerp(float a, float b, float t)
    {
        return (1.0f - t) * a + b + t;
    }

    /// <summary>
    /// Determines where a value lies between two points.
    /// </summary>
    /// <param name="a">The start of the range.</param>
    /// <param name="b">The end of the range.</param>
    /// <param name="v">The point within the range you want to calculate.</param>
    /// <returns>A value between zero and one, representing where the "value" parameter falls within the range defined by a and b.</returns>
    public static float InverseLerp(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }

    private static float ReMap(float input, float inputMin, float inputMax, float min, float max)
    {
        return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
    }

    /// <summary>
    /// Linearly interpolates between two points.
    /// </summary>
    /// <param name="a">Start value</param>
    /// <param name="b">End value</param>
    /// <param name="t">Value used to interpolate between a and b.</param>
    /// <returns>Interpolated value</returns>
    public static SystemVector3 Lerp(SystemVector3 a, SystemVector3 b, float t)
    {
        t = Clamp(t, 0, 1);

        var distance = new SystemVector3(b.X - a.X, b.Y - a.Y, b.Z - a.Z);

        var x = a.X + distance.X * t;
        var y = a.Y + distance.Y * t;
        var z = a.Z + distance.Z * t;

        return new SystemVector3(x, y, z);
    }

    /// <summary>
    /// Linearly interpolates between two points.
    /// </summary>
    /// <param name="a">Start value</param>
    /// <param name="b">End value</param>
    /// <param name="t">Value used to interpolate between a and b.</param>
    /// <returns>Interpolated value</returns>
    public static SystemVector2 Lerp(SystemVector2 a, SystemVector2 b, float t)
    {
        t = Clamp(t, 0, 1);

        var distance = new SystemVector2(b.X - a.X, b.Y - a.Y);

        var x = a.X + distance.X * t;
        var y = a.Y + distance.Y * t;

        return new SystemVector2(x, y);
    }

    /// <summary>
    /// Remap a vector from one range of values to another
    /// </summary>
    /// <param name="input">Input vector</param>
    /// <param name="inputMin">Minimum range of the input vector</param>
    /// <param name="inputMax">Maximum range of the input vector</param>
    /// <param name="outputMin">Minimum range of the output vector</param>
    /// <param name="outputMax">Maximum range of the output vector</param>
    /// <returns>V</returns>
    public static SystemVector2 ReMap(SystemVector2 input, SystemVector2 inputMin, SystemVector2 inputMax, SystemVector2 outputMin, SystemVector2 outputMax)
    {
        return new SystemVector2
        (
            ReMap(input.X, inputMin.X, inputMax.X, outputMin.X, outputMax.X),
            ReMap(input.Y, inputMin.Y, inputMax.Y, outputMin.Y, outputMax.Y)
        );
    }

    /// <summary>
    /// Create a rotated unit vector from degrees
    /// </summary>
    /// <param name="rotation">Degrees</param>
    /// <returns>Vector2 rotated unit vector</returns>
    public static SystemVector2 CreateRotatedUnitVector(float rotation)
    {
        return new SystemVector2(MathF.Cos(-rotation * (MathF.PI / 180)), MathF.Sin(-rotation * (MathF.PI / 180)));
    }

    /// <summary>
    /// Cross Product of two vectors
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">Second vector</param>
    /// <returns>The cross product</returns>
    public static float CrossProduct(SystemVector2 a, SystemVector2 b)
    {
        return a.X * b.Y - a.Y * b.X;
    }

    /// <summary>
    /// Cross Product of a vector and a value
    /// </summary>
    /// <param name="a">Vector</param>
    /// <param name="s">Value</param>
    /// <returns>The cross product</returns>
    public static SystemVector2 CrossProduct(SystemVector2 a, float s)
    {
        return new SystemVector2(s * a.Y, -s * a.X);
    }


    /// <summary>
    /// Cross Product of a vector and a value
    /// </summary>
    /// <param name="s">Value</param>
    /// <param name="a">Vector</param>
    /// <returns>The cross product</returns>
    public static SystemVector2 CrossProduct(float s, SystemVector2 a)
    {
        return new SystemVector2(-s * a.Y, s * a.X);
    }

    /// <summary>
    /// Get the length of a vector
    /// </summary>
    /// <param name="vector">Target vector</param>
    /// <returns>The length</returns>
    public static float Length(SystemVector2 vector)
    {
        return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
    }

    /// <summary>
    /// Find the magnitude of a vector
    /// </summary>
    /// <param name="vector">Target vector</param>
    /// <returns>Magnitude of the vector</returns>
    public static float Magnitude(SystemVector3 vector)
    {
        return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
    }

    /// <summary>
    /// Get the square length of a vector
    /// </summary>
    /// <param name="vector">Target vector</param>
    /// <returns>The square length of the vector</returns>
    public static float SqrLength(SystemVector2 vector)
    {
        return vector.X * vector.X + vector.Y * vector.Y;
    }

    /// <summary>
    /// Normalize a vector
    /// </summary>
    /// <param name="vector">Vector to normalize</param>
    /// <returns>The normalized vector</returns>
    public static SystemVector2 Normalized(SystemVector2 vector)
    {
        return vector * (1 / Length(vector));
    }
}