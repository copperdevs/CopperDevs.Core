using System;
using System.Collections.Generic;
using System.Linq;

namespace CopperDevs.Logger
{
    /// <summary>
    /// Core logger class
    /// </summary>
    public static class CopperLogger
    {
        /// <summary>
        /// Should timestamps be logged alongside the message
        /// </summary>
        public static bool IncludeTimestamps = true;

        /// <summary>
        /// Should exceptions logs include the full stack trace
        /// </summary>
        public static bool SimpleExceptions = false;

        internal static void LogMessage(AnsiColors.Names colorName, string prefix, object message, bool shouldLog)
        {
            if (shouldLog)
                LogMessage(colorName, colorName, prefix, message);
        }

        private static void LogMessage(AnsiColors.Names colorName, AnsiColors.Names backgroundColorName, string prefix, object message)
        {
            switch (message)
            {
                case Exception exception:
                    LogException(colorName, backgroundColorName, prefix, exception);
                    return;
                case List<string> list:
                    LogList(colorName, backgroundColorName, prefix, list);
                    return;
            }

            var color = AnsiColors.GetColor(colorName);
            var backgroundColor = AnsiColors.GetBackgroundColor(backgroundColorName);

            var time = IncludeTimestamps ? $"{DateTime.Now:HH:mm:ss}" : "";
            var timeSpacer = IncludeTimestamps ? " " : "";

            var timeText = $"{AnsiColors.Black}{AnsiColors.LightGrayBackground}{time}{AnsiColors.Reset}{AnsiColors.Black}{timeSpacer}";
            var prefixText = $"{backgroundColor}{prefix}:{AnsiColors.Reset}";

            Console.Write($"{timeText}{prefixText} {color}{message}{AnsiColors.Reset}{Environment.NewLine}");
        }

        private static void LogException(AnsiColors.Names colorName, AnsiColors.Names backgroundColorName, string prefix, Exception exception)
        {
            var lines = new List<string>
            {
                exception.Message,
                (SimpleExceptions ? $"{exception.TargetSite} in {exception.Source}" : string.Empty)
            };

            if (!SimpleExceptions)
            {
                lines.AddRange(exception.StackTrace.Split(
                    new[]
                    {
                        Environment.NewLine
                    },
                    StringSplitOptions.RemoveEmptyEntries)
                );
            }

            LogMessage(colorName, backgroundColorName, prefix, lines);
        }

        private static void LogList(AnsiColors.Names colorName, AnsiColors.Names backgroundColorName, string prefix, List<string> list)
        {
            var color = AnsiColors.GetColor(colorName);
            var backgroundColor = AnsiColors.GetBackgroundColor(backgroundColorName);

            var time = IncludeTimestamps ? $"{DateTime.Now:HH:mm:ss}" : "";
            var timeSpacer = IncludeTimestamps ? " " : "";

            var timeText = $"{AnsiColors.Black}{AnsiColors.LightGrayBackground}{time}{AnsiColors.Reset}{AnsiColors.Black}{timeSpacer}";
            var prefixText = $"{backgroundColor}{prefix}:{AnsiColors.Reset}";

            var finalResult = string.Empty;

            for (var i = 0; i < list.Count; i++)
            {
                var first = i == 0;
                var last = i == list.Count - 1;
                var item = list[i].TrimStart();

                if (!string.IsNullOrWhiteSpace(item))
                    finalResult += $"{(first ? string.Empty : string.Empty.PadLeft(prefixText.Length + 1))}{item}{(last ? string.Empty : Environment.NewLine)}";
            }
            
            Console.Write($"{timeText}{prefixText} {color}{finalResult}{AnsiColors.Reset}{Environment.NewLine}");
        }
    }
}