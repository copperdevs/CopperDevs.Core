namespace CopperDevs.Logger.Testing;

public static class Program
{
    public static void Main()
    {
        Log.Debug("debug moment");
        Log.Info("info moment");
        Log.Runtime("runtime moment");
        Log.Network("network moment");
        Log.Success("success moment");
        Log.Warning("warning moment");
        Log.Error("error moment");
        Log.Critical("critical moment");
        Log.Audit("audit moment");
        Log.Trace("trace moment");
        Log.Security("security moment");
        Log.UserAction("user action moment");
        Log.Performance("performance moment");
        Log.Config("config moment");
        Log.Fatal("fatal moment");

        try
        {
            RecursiveMoment(10, 0);
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