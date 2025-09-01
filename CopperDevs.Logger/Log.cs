using System;

// ReSharper disable MemberCanBePrivate.Global

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
        public static void Debug(object message) => Debug(message, true);

        /// <summary>
        /// Log a debug style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>
        public static void Debug(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.Gray, "Debug", message, condition);

        /// <summary>
        /// Log an info style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Info(object message) => Info(message, true);

        /// <summary>
        /// Log an info style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Info(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.Cyan, "Information", message, condition);

        /// <summary>
        /// Log a runtime style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Runtime(object message) => Runtime(message, true);

        /// <summary>
        /// Log a runtime style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Runtime(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.Magenta, "Runtime", message, condition);

        /// <summary>
        /// Log a network style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Network(object message) => Network(message, true);

        /// <summary>
        /// Log a network style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Network(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.Blue, "Network", message, condition);

        /// <summary>
        /// Log a success style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Success(object message) => Success(message, true);

        /// <summary>
        /// Log a success style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Success(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.BrightGreen, "Success", message, condition);

        /// <summary>
        /// Log a warning style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Warning(object message) => Warning(message, true);

        /// <summary>
        /// Log a warning style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Warning(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.BrightYellow, "Warning", message, condition);

        /// <summary>
        /// Log an error style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Error(object message) => Error(message, true);

        /// <summary>
        /// Log an error style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Error(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.Red, "Error", message, condition);

        /// <summary>
        /// Log a critical style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Critical(object message) => Critical(message, true);

        /// <summary>
        /// Log a critical style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Critical(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.BrightRed, "Critical", message, condition);

        /// <summary>
        /// Log an audit style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Audit(object message) => Audit(message, true);

        /// <summary>
        /// Log an audit style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Audit(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.Yellow, "Audit", message, condition);

        /// <summary>
        /// Log a trace style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Trace(object message) => Trace(message, true);

        /// <summary>
        /// Log a trace style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Trace(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.LightBlue, "Trace", message, condition);

        /// <summary>
        /// Log a security style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Security(object message) => Security(message, true);

        /// <summary>
        /// Log a security style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Security(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.Purple, "Security", message, condition);

        /// <summary>
        /// Log a user action style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void UserAction(object message) => UserAction(message, true);

        /// <summary>
        /// Log a user action style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void UserAction(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.CutePink, "User Action", message, condition);

        /// <summary>
        /// Log a performance style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Performance(object message) => Performance(message, true);

        /// <summary>
        /// Log a performance style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Performance(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.Pink, "Performance", message, condition);

        /// <summary>
        /// Log a config style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Config(object message) => Config(message, true);

        /// <summary>
        /// Log a config style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Config(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.LightGray, "Config", message, condition);

        /// <summary>
        /// Log a fatal style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        public static void Fatal(object message) => Fatal(message, true);

        /// <summary>
        /// Log a fatal style log to the console
        /// </summary>
        /// <param name="message">Data to log</param>
        /// <param name="condition">Should the message actually log</param>A
        public static void Fatal(object message, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.DarkRed, "Fatal", message, condition);

        /// <summary>
        /// Log an exception to the console
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public static void Exception(Exception exception) => Exception(exception, true);

        /// <summary>
        /// Log an exception to the console
        /// </summary>
        /// <param name="exception">Exception to log</param>
        /// <param name="condition">Should the message actually log</param>
        public static void Exception(Exception exception, bool condition) => CopperLogger.LogMessage(AnsiColors.Names.Red, "Exception", exception, condition);
    }
}