using System.Reflection;

namespace CopperDevs.Core.Utility;

/// <summary>
/// Simple object pool for types
/// </summary>
/// <typeparam name="T">Type of object to pool</typeparam>
public static class ObjectPool<T> where T : new()
{
    private static readonly MethodInfo GottenMethod = typeof(T).GetMethod("Gotten")!;
    private static readonly MethodInfo ReleasedMethod = typeof(T).GetMethod("Released")!;

    private static readonly Stack<T> Pool = new();

    /// <summary>
    /// Get an object from the pool if one is available, otherwise it creates a new one
    /// </summary>
    /// <returns>The object from the pool</returns>
    public static T Get()
    {
        var obj = Pool.Count > 0 ? Pool.Pop() : new T();
        PoolableUpdate(obj, PoolableUpdateType.Gotten);
        return obj;
    }

    /// <summary>
    /// Returns an object to the pool to be reused
    /// </summary>
    /// <param name="obj">Object to return</param>
    public static void Return(T obj)
    {
        PoolableUpdate(obj, PoolableUpdateType.Released);
        Pool.Push(obj);
    }

    private static void PoolableUpdate(T obj, PoolableUpdateType updateType)
    {
        if (!typeof(T).Implements<IObjectPoolable>())
            return;

        switch (updateType)
        {
            case PoolableUpdateType.Gotten:
                GottenMethod.Invoke(obj, []);
                break;
            case PoolableUpdateType.Released:
                ReleasedMethod.Invoke(obj, []);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
        }
    }

    private enum PoolableUpdateType
    {
        Gotten,
        Released
    }
}