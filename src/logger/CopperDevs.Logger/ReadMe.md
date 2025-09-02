# CopperDevs.Logger

> Simple, opinionated, logger utility

## Logging Options

- Include Timestamps: Should timestamps be logged alongside the message
    - bool
- Simple Exceptions: Should exceptions logs include the full stack trace
    - bool
- List Log Type: How lists should be printed to console
    - [Direct](#list-log-type---direct-code-example): Directly convert the list object to a string
    - [Multiple](#list-log-type---multiple-code-example): Print each value of the list to its own line
    - [Single](#list-log-type---single-code-example): Formats the values of the list into a single line

## Log Types

### Code Example

```csharp
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
```

### Logged Result

![Console logged rendered result](https://raw.githubusercontent.com/copperdevs/CopperDevs.Core/refs/heads/master/src/logger/repo/base-log-types.png)

## Exception Logging

### Code Example

```csharp
try
{
    // having some recursion to add depth to the stack trace
    RecursiveExample(6, 0);
}
catch (Exception e)
{
    Log.Exception(e);
}

    
public static void RecursiveExample(int maxDepth, int currentDepth)
{
    if (currentDepth == 0)
        Log.Info($"Starting recursive loop with depth of {maxDepth}");

    Log.Debug($"Current depth of {currentDepth}");

    if (maxDepth >= currentDepth)
    {
        currentDepth++;
        
        RecursiveExample(maxDepth, currentDepth);
    }
    else
    {
        Log.Success($"Finished");

        throw new NullReferenceException("exception core");
    }
}
```

### Logged Result

![Console logged rendered result](https://raw.githubusercontent.com/copperdevs/CopperDevs.Core/refs/heads/master/src/logger/repo/exception-logging.png)

## List Logging

### List Log Type - Direct Code Example

```csharp
CopperLogger.ListLogType = ListLogType.Direct;
Log.Config($"Setting {nameof(CopperLogger.ListLogType)} to {ListLogType.Direct}");
        
Log.Debug(new List<string> { "list", "logging", "moment" });
Log.Debug(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
Log.Debug(new[] { "list", "logging", "moment" });
Log.Debug(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
```

### List Log Type - Direct Code Result

![Console logged rendered result](https://raw.githubusercontent.com/copperdevs/CopperDevs.Core/refs/heads/master/src/logger/repo/direct-list-log-type.png)

### List Log Type - Multiple Code Example

```csharp
CopperLogger.ListLogType = ListLogType.Multiple;
Log.Config($"Setting {nameof(CopperLogger.ListLogType)} to {ListLogType.Multiple}");
        
Log.Debug(new List<string> { "list", "logging", "moment" });
Log.Debug(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
Log.Debug(new[] { "list", "logging", "moment" });
Log.Debug(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
```

### List Log Type - Multiple Code Result

![Console logged rendered result](https://raw.githubusercontent.com/copperdevs/CopperDevs.Core/refs/heads/master/src/logger/repo/multiple-list-log-type.png)

### List Log Type - Single Code Example

```csharp
CopperLogger.ListLogType = ListLogType.Single;
Log.Config($"Setting {nameof(CopperLogger.ListLogType)} to {ListLogType.Single}");
        
Log.Debug(new List<string> { "list", "logging", "moment" });
Log.Debug(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
Log.Debug(new[] { "list", "logging", "moment" });
Log.Debug(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
```

### List Log Type - Single Code Result

![Console logged rendered result](https://raw.githubusercontent.com/copperdevs/CopperDevs.Core/refs/heads/master/src/logger/repo/single-list-log-type.png)
