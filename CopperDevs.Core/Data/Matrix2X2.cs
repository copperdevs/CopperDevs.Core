using System.Numerics;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CopperDevs.Core.Data;

// https://www.codeproject.com/Articles/1029858/Making-a-D-Physics-Engine-The-Math
/// <summary>
/// Represents a 2x2 matrix.
/// </summary>
public struct Matrix2X2
{
    /// <summary>
    /// The first element of the first row
    /// </summary>
    public float M00;
    
    /// <summary>
    /// The second element of the first row
    /// </summary>
    public float M01;
    
    /// <summary>
    /// The first element of the second row
    /// </summary>
    public float M10;
    
    /// <summary>
    /// The second element of the second row
    /// </summary>
    public float M11;

    /// <summary>
    /// Create a new 2x2 matrix with all values set
    /// </summary>
    /// <param name="newM00">Value of the first element of the first row</param>
    /// <param name="newM01">Value of the second element of the first row</param>
    /// <param name="newM10">Value of the first element of the second row</param>
    /// <param name="newM11">Value of the second element of the second row</param>
    public Matrix2X2(float newM00, float newM01, float newM10, float newM11)
    {
        M00 = newM00;
        M01 = newM01;
        M10 = newM10;
        M11 = newM11;
    }

    /// <summary>
    /// Set the matrix rotation from radians
    /// </summary>
    /// <param name="radians">Radians value</param>
    public void Set(float radians)
    {
        var c = MathF.Cos(radians);
        var s = MathF.Sin(radians);

        M00 = c;
        M01 = -s;
        M10 = s;
        M11 = c;
    }

    /// <summary>
    /// Create a transposed matrix
    /// </summary>
    /// <returns>The created matrix</returns>
    public Matrix2X2 Transpose()
    {
        return new Matrix2X2(M00, M10, M01, M11);
    }
    public static Vector2 operator *(Matrix2X2 matrix, Vector2 vector)
    {
        return new Vector2(matrix.M00 * vector.X + matrix.M01 * vector.Y, matrix.M10 * vector.X + matrix.M11 * vector.Y);
    }

    public static Matrix2X2 operator *(Matrix2X2 a, Matrix2X2 b)
    {
        return new Matrix2X2(
            a.M00 * b.M00 + a.M01 * b.M10, a.M00 * b.M01 + a.M01 * b.M11,
            a.M10 * b.M00 + a.M11 * b.M10, a.M10 * b.M01 + a.M11 * b.M11);
    }
}