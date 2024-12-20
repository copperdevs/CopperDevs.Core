namespace CopperDevs.Core.Utility;

/// <summary>
/// Simple object pool for types
/// </summary>
/// <typeparam name="T">Type of object to pool</typeparam>
public static class ObjectPool<T> where T : IObjectPoolable, new()
{
    private static readonly Stack<T> Pool = new Stack<T>();

    /// <summary>
    /// Get an object from the pool if one is available, otherwise it creates a new one
    /// </summary>
    /// <returns>The object from the pool</returns>
    public static T Get()
    {
        var obj = Pool.Count > 0 ? Pool.Pop() : new T();
        obj.Gotten();
        return obj;
    }

    /// <summary>
    /// Returns an object to the pool to be reused
    /// </summary>
    /// <param name="obj">Object to return</param>
    public static void Return(T obj)
    {
        obj.Released();
        Pool.Push(obj);
    }
}