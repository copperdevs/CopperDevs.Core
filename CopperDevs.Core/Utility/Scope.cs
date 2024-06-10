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
            Log.Warning($"{GetType().Name} was not disposed! You should use the 'using' keyword or manually call Dispose.");
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract void CloseScope();
}