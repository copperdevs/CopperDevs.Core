#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace CopperDevs.Core.Utility;

public abstract class Scope : IDisposable
{
    private bool disposed;

    internal virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;
        if (disposing)
            CloseScope();
        disposed = true;
    }

    ~Scope()
    {
        if (!disposed)
            throw new Exception($"{GetType().Name} was not disposed! You should use the 'using' keyword or manually call Dispose.");
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract void CloseScope();
}
