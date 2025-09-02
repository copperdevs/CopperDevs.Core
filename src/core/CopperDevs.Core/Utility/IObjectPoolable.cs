namespace CopperDevs.Core.Utility;

/// <summary>
/// Allows a class to be added to an object pool
/// </summary>
public interface IObjectPoolable
{
    /// <summary>
    /// Called when this object is taken from an <see cref="ObjectPool{T}"/> 
    /// </summary>
    void Gotten();

    /// <summary>
    /// Called when this object is added to an <see cref="ObjectPool{T}"/> 
    /// </summary>
    void Released();
}