using System.Numerics;

namespace CopperDevs.Core.Data;

// https://www.codeproject.com/Articles/1029858/Making-a-D-Physics-Engine-The-Math
public struct Matrix2X2
{
    private float m00, m01;
    private float m10, m11;

    private Matrix2X2(float newM00, float newM01, float newM10, float newM11)
    {
        m00 = newM00;
        m01 = newM01;
        m10 = newM10;
        m11 = newM11;
    }

    public void Set(float radians)
    {
        var c = MathF.Cos(radians);
        var s = MathF.Sin(radians);

        m00 = c;
        m01 = -s;
        m10 = s;
        m11 = c;
    }

    public Matrix2X2 Transpose()
    {
        return new Matrix2X2(m00, m10, m01, m11);
    }

    public static Vector2 operator *(Matrix2X2 matrix, Vector2 vector)
    {
        return new Vector2(matrix.m00 * vector.X + matrix.m01 * vector.Y, matrix.m10 * vector.X + matrix.m11 * vector.Y);
    }

    public static Matrix2X2 operator *(Matrix2X2 a, Matrix2X2 b)
    {
        return new Matrix2X2(
            a.m00 * b.m00 + a.m01 * b.m10, a.m00 * b.m01 + a.m01 * b.m11,
            a.m10 * b.m00 + a.m11 * b.m10, a.m10 * b.m01 + a.m11 * b.m11);
    }
}