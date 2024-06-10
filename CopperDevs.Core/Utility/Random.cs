using System.Numerics;
using SystemRandom = System.Random;

namespace CopperDevs.Core.Utility;

/// <summary>
/// Custom random class layered over the built in system random
/// </summary>
public static class Random
{
    private static readonly SystemRandom SystemRandom = new(new Guid().GetHashCode());

    /// <summary>
    /// Get a random value inside of a unit circle
    /// </summary>
    public static Vector2 InsideUnitCircle
    {
        get
        {
            var theta = Range(0, 1f) * 2 * MathF.PI;
            return new Vector2(MathF.Cos(theta), MathF.Sin(theta) * MathF.Sqrt(Range(0, 1f)));
        }
    }

    /// <summary>
    /// Get a random float in-between two values
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <returns>A random number</returns>
    public static float Range(float min, float max)
    {
        if ((int)(MathF.Round(min * 1000) / 1000) == (int)(MathF.Round(max * 1000) / 1000))
            return (min + max) / 2;

        var lowerLimit = MathF.Min(min, max);
        var upperLimit = MathF.Max(min, max);

        return SystemRandom.NextSingle() * (upperLimit - lowerLimit) + lowerLimit;
    }

    /// <summary>
    /// Get a random int in-between two values
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <returns>A random number</returns>
    public static int Range(int min, int max)
    {
        if (min == max)
            return (min + max) / 2;

        var lowerLimit = Math.Min(min, max);
        var upperLimit = Math.Max(min, max);

        return SystemRandom.Next(lowerLimit, upperLimit);
    }

    /// <summary>
    /// Get a random float in-between zero and a value
    /// </summary>
    /// <param name="max">Maximum value</param>
    /// <returns>A random number</returns>
    public static float Range(float max)
    {
        return Range(0, max);
    }

    /// <summary>
    /// Get a random int in-between zero and a value
    /// </summary>
    /// <param name="max">Maximum value</param>
    /// <returns>A random number</returns>
    public static int Range(int max)
    {
        return Range(0, max);
    }

    /// <summary>
    /// Get a random float between two values
    /// </summary>
    /// <param name="valueRange">Vector range input</param>
    /// <returns>A random number</returns>
    /// <remarks>X is the minimum range and Y is the maximum range</remarks>
    public static float Range(Vector2 valueRange)
    {
        return Range(valueRange.X, valueRange.Y);
    }

    /// <summary>
    /// Get a random value from a list
    /// </summary>
    /// <param name="items">List items</param>
    /// <typeparam name="T">Type of item in the list</typeparam>
    /// <returns>Random item from the list</returns>
    public static T Item<T>(List<T> items)
    {
        return items[SystemRandom.Next(items.Count)];
    }

    /// <summary>
    /// Get a random value from a list
    /// </summary>
    /// <param name="items">List items</param>
    /// <param name="defaultValue">Default value returned when the list is empty</param>
    /// <typeparam name="T">Type of item in the list</typeparam>
    /// <returns>Random item from the list</returns>
    public static T Item<T>(List<T> items, T defaultValue)
    {
        return items.Count is 0 ? defaultValue : items[SystemRandom.Next(items.Count - 1)];
    }

    /// <summary>
    /// Get a random point inside of an annulus
    /// </summary>
    /// <param name="origin">Center of the annulus</param>
    /// <param name="minRadius">Minimum range of the annulus</param>
    /// <param name="maxRadius">Maximum range of the annulus</param>
    /// <returns>Random point inside of the annulus</returns>
    public static Vector2 PointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
    {
        var randomDirection = Vector2.Normalize(InsideUnitCircle * origin);
        var randomDistance = Range(minRadius, maxRadius);
        return origin + randomDirection * randomDistance;
    }

    /// <summary>
    /// Get a random point inside of an annulus
    /// </summary>
    /// <param name="origin">Center of the annulus</param>
    /// <param name="radius">Vector range input</param>
    /// <returns>Random point inside of the annulus</returns>
    /// <remarks>X is the minimum range and Y is the maximum range</remarks>
    public static Vector2 PointInAnnulus(Vector2 origin, Vector2 radius)
    {
        return PointInAnnulus(origin, radius.X, radius.Y);
    }
}