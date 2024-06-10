using System.Numerics;

namespace CopperDevs.Core.Utility;

public static class Extensions
{
    public static string ToTitleCase(this string target) => TextUtil.ConvertToTitleCase(target);
    public static Vector4 ToVector(this Quaternion quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    public static Quaternion ToQuaternion(this Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
    public static Quaternion FromEulerAngles(this Vector3 euler) => MathUtil.FromEulerAngles(euler);
    public static Vector3 ToEulerAngles(this Quaternion quaternion) => MathUtil.ToEulerAngles(quaternion);
    public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max) => MathUtil.Clamp(value, min, max);
    public static Vector2 ToRotatedUnitVector(this float value) => MathUtil.CreateRotatedUnitVector(value);
    public static Vector2 WithX(this Vector2 vector, float value) => vector with { X = value };
    public static Vector2 WithY(this Vector2 vector, float value) => vector with { Y = value };
    public static Vector3 WithX(this Vector3 vector, float value) => vector with { X = value };
    public static Vector3 WithY(this Vector3 vector, float value) => vector with { Y = value };
    public static Vector3 WithZ(this Vector3 vector, float value) => vector with { Z = value };
    public static Vector4 WithX(this Vector4 vector, float value) => vector with { X = value };
    public static Vector4 WithY(this Vector4 vector, float value) => vector with { Y = value };
    public static Vector4 WithZ(this Vector4 vector, float value) => vector with { Z = value };
    public static Vector4 WithW(this Vector4 vector, float value) => vector with { W = value };
    public static Vector2 FlipX(this Vector2 vector) => vector with { X = -vector.X };
    public static Vector2 FlipY(this Vector2 vector) => vector with { Y = -vector.Y };
    public static Vector3 FlipX(this Vector3 vector) => vector with { X = -vector.X };
    public static Vector3 FlipY(this Vector3 vector) => vector with { Y = -vector.Y };
    public static Vector3 FlipZ(this Vector3 vector) => vector with { Z = -vector.Z };
    public static Vector4 FlipX(this Vector4 vector) => vector with { X = -vector.X };
    public static Vector4 FlipY(this Vector4 vector) => vector with { Y = -vector.Y };
    public static Vector4 FlipZ(this Vector4 vector) => vector with { Z = -vector.Z };
    public static Vector4 FlipW(this Vector4 vector) => vector with { W = -vector.W };
    public static Vector3 ToVector3(this Vector2 vector, float z = 0) => new(vector.X, vector.Y, z);
    public static Vector4 ToVector4(this Vector2 vector, float z = 0, float w = 0) => new(vector.X, vector.Y, z, w);
    public static Vector4 ToVector4(this Vector3 vector, float w = 0) => new(vector.X, vector.Y, vector.Z, w);
    public static Vector2 ToVector2<T>(this T value) where T : INumber<T> => new((float)Convert.ChangeType(value, typeof(float)));
    public static Vector3 ToVector3<T>(this T value) where T : INumber<T> => new((float)Convert.ChangeType(value, typeof(float)));
    public static Vector4 ToVector4<T>(this T value) where T : INumber<T> => new((float)Convert.ChangeType(value, typeof(float)));
    public static string ToFancyString(this IEnumerable<byte> array) => array.Aggregate("", (current, item) => current + $"<{item}>,");

    public static string CapitalizeFirstLetter(this string message)
    {
        return message.Length switch
        {
            0 => "",
            1 => char.ToUpper(message[0]).ToString(),
            _ => char.ToUpper(message[0]) + message[1..]
        };
    }

    public static Vector2 Remap(this Vector2 input, Vector2 inputMin, Vector2 inputMax, Vector2 outputMin, Vector2 outputMax) =>
        MathUtil.ReMap(input, inputMin, inputMax, outputMin, outputMax);

    public static Matrix4x4 ToRowMajor(this Matrix4x4 columnMatrix)
    {
        return new Matrix4x4(
            columnMatrix.M11, columnMatrix.M21, columnMatrix.M31, columnMatrix.M41,
            columnMatrix.M12, columnMatrix.M22, columnMatrix.M32, columnMatrix.M42,
            columnMatrix.M13, columnMatrix.M23, columnMatrix.M33, columnMatrix.M43,
            columnMatrix.M14, columnMatrix.M24, columnMatrix.M34, columnMatrix.M44
        );
    }

    public static Matrix4x4 ToColumnMajor(this Matrix4x4 rowMatrix)
    {
        return new Matrix4x4(
            rowMatrix.M11, rowMatrix.M12, rowMatrix.M13, rowMatrix.M14,
            rowMatrix.M21, rowMatrix.M22, rowMatrix.M23, rowMatrix.M24,
            rowMatrix.M31, rowMatrix.M32, rowMatrix.M33, rowMatrix.M34,
            rowMatrix.M41, rowMatrix.M42, rowMatrix.M43, rowMatrix.M44
        );
    }

    public static int EnumToInt<T>(this T value) where T : Enum
    {
        return (int)(object)value;
    }

    public static bool HasAttribute<T>(this object value) where T : Attribute
    {
        return value.GetType().IsDefined(typeof(T), false);
    }

    public static bool HasAttribute<T>(this Type value) where T : Attribute
    {
        return value.IsDefined(typeof(T), false);
    }

    public static bool IsSameOrSubclass(this Type potentialBase, Type potentialDescendant)
    {
        return potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
    }

    public static int ToInt(this bool value)
    {
        return value ? 1 : 0;
    }
}