using System;

namespace CopperDevs.Logger
{
    /// <summary>
    /// Main log class that holds all the preset log methods
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Log a debug style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Debug(object message) => CopperLogger.LogMessage(AnsiColors.Names.Gray, "Debug", message);

        /// <summary>
        /// Log an info style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Info(object message) => CopperLogger.LogMessage(AnsiColors.Names.Cyan, "Information", message);

        /// <summary>
        /// Log a runtime style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Runtime(object message) => CopperLogger.LogMessage(AnsiColors.Names.Magenta, "Runtime", message);

        /// <summary>
        /// Log a network style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Network(object message) => CopperLogger.LogMessage(AnsiColors.Names.Blue, "Network", message);

        /// <summary>
        /// Log a success style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Success(object message) => CopperLogger.LogMessage(AnsiColors.Names.BrightGreen, "Success", message);

        /// <summary>
        /// Log a warning style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Warning(object message) => CopperLogger.LogMessage(AnsiColors.Names.BrightYellow, "Warning", message);

        /// <summary>
        /// Log a error style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Error(object message) => CopperLogger.LogMessage(AnsiColors.Names.Red, "Error", message);

        /// <summary>
        /// Log a critical style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Critical(object message) => CopperLogger.LogMessage(AnsiColors.Names.BrightRed, "Critical", message);

        /// <summary>
        /// Log a audit style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Audit(object message) => CopperLogger.LogMessage(AnsiColors.Names.Yellow, "Audit", message);

        /// <summary>
        /// Log a trace style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Trace(object message) => CopperLogger.LogMessage(AnsiColors.Names.LightBlue, "Trace", message);

        /// <summary>
        /// Log a security style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Security(object message) => CopperLogger.LogMessage(AnsiColors.Names.Purple, "Security", message);

        /// <summary>
        /// Log a user action style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void UserAction(object message) => CopperLogger.LogMessage(AnsiColors.Names.CutePink, "User Action", message);

        /// <summary>
        /// Log a performance style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Performance(object message) => CopperLogger.LogMessage(AnsiColors.Names.Pink, "Performance", message);

        /// <summary>
        /// Log a config style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Config(object message) => CopperLogger.LogMessage(AnsiColors.Names.LightGray, "Config", message);

        /// <summary>
        /// Log a fatal style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Fatal(object message) => CopperLogger.LogMessage(AnsiColors.Names.DarkRed, "Fatal", message);

        /// <summary>
        /// Log an exception to the console
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public static void Exception(Exception exception) => CopperLogger.LogMessage(AnsiColors.Names.Red, "Exception", exception);
    }
}