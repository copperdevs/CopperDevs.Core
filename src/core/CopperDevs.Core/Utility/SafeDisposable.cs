#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace CopperDevs.Core.Utility;

public abstract class SafeDisposable : IDisposable
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once MemberCanBePrivate.Global
    protected bool hasDisposed = false;

    ~SafeDisposable()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool manual)
    {
        if (hasDisposed)
            return;

        hasDisposed = true;
        DisposeResources();
    }

    public abstract void DisposeResources();
}