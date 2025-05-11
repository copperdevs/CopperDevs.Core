namespace CopperDevs.Core.Utility;

/// <summary>
/// Simple object pool for types
/// </summary>
/// <typeparam name="T">Type of object to pool</typeparam>
public class ObjectPool<T> where T : IObjectPoolable, new()
{
    private readonly Stack<T> pool = new();

    /// <summary>
    /// Get an object from the pool if one is available, otherwise it creates a new one
    /// </summary>
    /// <returns>The object from the pool</returns>
    public T Get()
    {
        var obj = pool.Count > 0 ? pool.Pop() : new T();
        obj.Gotten();
        return obj;
    }

    /// <summary>
    /// Returns an object to the pool to be reused
    /// </summary>
    /// <param name="obj">Object to return</param>
    public void Return(T obj)
    {
        obj.Released();
        pool.Push(obj);
    }
}