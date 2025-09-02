namespace CopperDevs.Logger.Testing;

public static class Program
{
    public static void Main()
    {
        Log.Debug("Debug log example");
        Log.Info("Info log example");
        Log.Runtime("Runtime log example");
        Log.Network("Network log example");
        Log.Success("Success log example");
        Log.Warn("Warning log example");
        Log.Error("Error log example");
        Log.Critical("Critical log example");
        Log.Audit("Audit log example");
        Log.Trace("Trace log example");
        Log.Security("Security log example");
        Log.UserAction("User Action log example");
        Log.Performance("Performance log example");
        Log.Config("Config log example");
        Log.Fatal("Fatal log example");

        try
        {
            // having some recursion to add depth to the stack trace
            RecursiveMoment(6, 0);
        }
        catch (Exception e)
        {
            Log.Exception(e);
        }

        LogLists(ListLogType.Direct);
        LogLists(ListLogType.Multiple);
        LogLists(ListLogType.Single);
    }

    private static void LogLists(ListLogType listType)
    {
        CopperLogger.ListLogType = listType;
        Log.Config($"Setting {nameof(CopperLogger.ListLogType)} to {listType}");
        
        Log.Debug(new List<string> { "list", "logging", "moment" });
        Log.Debug(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
#pragma warning disable CA1861
        Log.Debug(new[] { "list", "logging", "moment" });
        Log.Debug(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
#pragma warning restore CA1861
    }

    public static void RecursiveMoment(int maxDepth, int currentDepth)
    {
        if (currentDepth == 0)
            Log.Info($"Starting recursive loop with depth of {maxDepth}");

        Log.Debug($"Current depth of {currentDepth}");

        if (maxDepth >= currentDepth)
        {
            currentDepth++;
            // ReSharper disable once TailRecursiveCall
            RecursiveMoment(maxDepth, currentDepth);
        }
        else
        {
            Log.Success($"Finished");

            throw new NullReferenceException("exception core");
        }
    }
}