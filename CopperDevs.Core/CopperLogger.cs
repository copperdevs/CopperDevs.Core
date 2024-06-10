using static CopperDevs.Core.Data.ConsoleColors;

namespace CopperDevs.Core;

public static class Log
{
    public static void Debug(object message) => CopperLogger.Log(Names.Gray, "Debug", message);

    public static void Info(object message) => CopperLogger.Log(Names.Cyan, "Information", message);

    public static void Runtime(object message) => CopperLogger.Log(Names.Magenta, "Runtime", message);

    public static void Network(object message) => CopperLogger.Log(Names.Blue, "Network", message);

    public static void Success(object message) => CopperLogger.Log(Names.BrightGreen, "Success", message);

    public static void Warning(object message) => CopperLogger.Log(Names.BrightYellow, "Warning", message);

    public static void Error(object message) => CopperLogger.Log(Names.Red, "Error", message);

    public static void Critical(object message) => CopperLogger.Log(Names.BrightRed, "Critical", message);

    public static void Audit(object message) => CopperLogger.Log(Names.Yellow, "Audit", message);

    public static void Trace(object message) => CopperLogger.Log(Names.LightBlue, "Trace", message);

    public static void Security(object message) => CopperLogger.Log(Names.Purple, "Security", message);

    public static void UserAction(object message) => CopperLogger.Log(Names.CutePink, "User Action", message);

    public static void Performance(object message) => CopperLogger.Log(Names.Pink, "Performance", message);

    public static void Config(object message) => CopperLogger.Log(Names.LightGray, "Config", message);

    public static void Fatal(object message) => CopperLogger.Log(Names.DarkRed, "Fatal", message);

    public static void Exception(Exception exception) => CopperLogger.Log(Names.Red, "Exception", exception);
}

public static class CopperLogger
{
    public static bool IncludeTimestamps = true;

    public static void Log(Names colorName, string prefix, object message)
    {
        var color = GetColor(colorName);
        var backgroundColor = GetBackgroundColor(colorName);

        var time = IncludeTimestamps ? $"{DateTime.Now:HH:mm:ss}" : "";
        var timeSpacer = IncludeTimestamps ? " " : "";

        Console.Write($"{Black}{LightGrayBackground}{time}{Reset}{Black}{timeSpacer}{backgroundColor}{prefix}:{Reset} {color}{message}{Reset}{Environment.NewLine}");
    }
}