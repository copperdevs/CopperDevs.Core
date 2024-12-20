using System.Numerics;
using System.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CopperDevs.Core.Utility;

public static class Extensions
{
    public static string ToTitleCase(this string target) => TextUtil.ConvertToTitleCase(target);
    public static string ToFancyString(this IEnumerable<byte> array) => array.Aggregate("", (current, item) => current + $"<{item}>,");

    public static SystemVector4 ToVector(this Quaternion quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    public static Quaternion ToQuaternion(this SystemVector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);

    public static Quaternion FromEulerAngles(this SystemVector3 euler) => MathUtil.FromEulerAngles(euler);
    public static SystemVector3 ToEulerAngles(this Quaternion quaternion) => MathUtil.ToEulerAngles(quaternion);

    public static SystemVector3 Clamp(this SystemVector3 value, SystemVector3 min, SystemVector3 max) => MathUtil.Clamp(value, min, max);

    public static SystemVector2 ToRotatedUnitVector(this float value) => MathUtil.CreateRotatedUnitVector(value);

    public static SystemVector2 WithX(this SystemVector2 vector, float value) => vector with { X = value };
    public static SystemVector2 WithY(this SystemVector2 vector, float value) => vector with { Y = value };

    public static SystemVector3 WithX(this SystemVector3 vector, float value) => vector with { X = value };
    public static SystemVector3 WithY(this SystemVector3 vector, float value) => vector with { Y = value };
    public static SystemVector3 WithZ(this SystemVector3 vector, float value) => vector with { Z = value };

    public static SystemVector4 WithX(this SystemVector4 vector, float value) => vector with { X = value };
    public static SystemVector4 WithY(this SystemVector4 vector, float value) => vector with { Y = value };
    public static SystemVector4 WithZ(this SystemVector4 vector, float value) => vector with { Z = value };
    public static SystemVector4 WithW(this SystemVector4 vector, float value) => vector with { W = value };

    public static SystemVector2 FlipX(this SystemVector2 vector) => vector with { X = -vector.X };
    public static SystemVector2 FlipY(this SystemVector2 vector) => vector with { Y = -vector.Y };

    public static SystemVector3 FlipX(this SystemVector3 vector) => vector with { X = -vector.X };
    public static SystemVector3 FlipY(this SystemVector3 vector) => vector with { Y = -vector.Y };
    public static SystemVector3 FlipZ(this SystemVector3 vector) => vector with { Z = -vector.Z };

    public static SystemVector4 FlipX(this SystemVector4 vector) => vector with { X = -vector.X };
    public static SystemVector4 FlipY(this SystemVector4 vector) => vector with { Y = -vector.Y };
    public static SystemVector4 FlipZ(this SystemVector4 vector) => vector with { Z = -vector.Z };
    public static SystemVector4 FlipW(this SystemVector4 vector) => vector with { W = -vector.W };

    public static SystemVector3 ToVector3(this SystemVector2 vector, float z = 0) => new(vector.X, vector.Y, z);
    public static SystemVector4 ToVector4(this SystemVector2 vector, float z = 0, float w = 0) => new(vector.X, vector.Y, z, w);
    public static SystemVector4 ToVector4(this SystemVector3 vector, float w = 0) => new(vector.X, vector.Y, vector.Z, w);

    public static SystemVector2 ToVector2<T>(this T value) where T : INumber<T> => new((float)Convert.ChangeType(value, typeof(float)));
    public static SystemVector3 ToVector3<T>(this T value) where T : INumber<T> => new((float)Convert.ChangeType(value, typeof(float)));
    public static SystemVector4 ToVector4<T>(this T value) where T : INumber<T> => new((float)Convert.ChangeType(value, typeof(float)));

    public static string CapitalizeFirstLetter(this string message)
    {
        return message.Length switch
        {
            0 => "",
            1 => char.ToUpper(message[0]).ToString(),
            _ => char.ToUpper(message[0]) + message[1..]
        };
    }

    public static SystemVector2 Remap(this SystemVector2 input, SystemVector2 inputMin, SystemVector2 inputMax, SystemVector2 outputMin, SystemVector2 outputMax)
    {
        return MathUtil.ReMap(input, inputMin, inputMax, outputMin, outputMax);
    }

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

    public static int ToInt<T>(this T value) where T : Enum
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

    /// <summary>
    /// Get all public static values from a certain value
    /// </summary>
    /// <param name="type">Type of the class to get the values from</param>
    /// <typeparam name="T">Field type</typeparam>
    /// <returns>List of found types</returns>
    public static List<T> GetAllPublicConstantValues<T>(this Type type)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi is { IsLiteral: true, IsInitOnly: false } && fi.FieldType == typeof(T))
            .Select(x => (T)x.GetRawConstantValue()!)
            .ToList();
    }


    public static Vector2 WithoutX(this Vector3 vector) => new(vector.Y, vector.Z);

    public static Vector2 WithoutY(this Vector3 vector) => new(vector.X, vector.Z);

    public static Vector2 WithoutZ(this Vector3 vector) => new(vector.X, vector.Y);

    public static Vector3 WithoutX(this Vector4 vector) => new(vector.Y, vector.Z, vector.W);

    public static Vector3 WithoutY(this Vector4 vector) => new(vector.X, vector.Z, vector.W);

    public static Vector3 WithoutZ(this Vector4 vector) => new(vector.X, vector.Y, vector.W);

    public static Vector3 WithoutW(this Vector4 vector) => new(vector.X, vector.Y, vector.Z);

    public static bool Implements<TI>(this Type source) where TI : class
    {
        return typeof(TI).IsAssignableFrom(source);
    }
}