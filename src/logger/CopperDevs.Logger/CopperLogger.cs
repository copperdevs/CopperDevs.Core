using System;
using System.Collections;
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
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once ConvertToConstant.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static bool IncludeTimestamps = true;

        /// <summary>
        /// Should exceptions logs include the full stack trace
        /// </summary>
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once ConvertToConstant.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static bool SimpleExceptions = false;

        /// <summary>
        /// How lists should be printed to console
        /// </summary>
        public static ListLogType ListLogType = ListLogType.Multiple;

        internal static void LogMessage(AnsiColors.Names colorName, string prefix, object message, bool shouldLog)
        {
            if (shouldLog)
                LogMessage(colorName, colorName, prefix, message);
        }

        private static void LogMessage(AnsiColors.Names colorName, AnsiColors.Names backgroundColorName, string prefix, object message)
        {
            if (HandleList(colorName, backgroundColorName, prefix, message)) return;

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

        private static bool HandleList(AnsiColors.Names colorName, AnsiColors.Names backgroundColorName, string prefix, object message)
        {
            var isCollection = (
                                   message.GetType() is { IsGenericType: true } &&
                                   message.GetType().GetGenericTypeDefinition() == typeof(List<>)
                               )
                               || message.GetType().IsArray;

            if (!isCollection)
                return false;

            if (ListLogType == ListLogType.Direct)
            {
                string moment = $"{message}";
                LogMessage(colorName, backgroundColorName, prefix, moment);
                return true;
            }

            // we properly log this specific case elsewhere, so just return from here
            if (message.GetType() == typeof(List<string>)) return false;

            var stringList = (from object? item in ((IList)message) select item.ToString()).ToList();

            LogMessage(colorName, backgroundColorName, prefix, stringList);
            return true;
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

            var current = ListLogType;
            ListLogType = ListLogType.Multiple;
            LogMessage(colorName, backgroundColorName, prefix, lines);
            ListLogType = current;
        }

        private static void LogList(AnsiColors.Names colorName, AnsiColors.Names backgroundColorName, string prefix, List<string> list)
        {
            var color = AnsiColors.GetColor(colorName);
            var backgroundColor = AnsiColors.GetBackgroundColor(backgroundColorName);

            var time = IncludeTimestamps ? $"{DateTime.Now:HH:mm:ss}" : "";
            var timeSpacer = IncludeTimestamps ? " " : "";

            var timeText = $"{AnsiColors.Black}{AnsiColors.LightGrayBackground}{time}{AnsiColors.Reset}{AnsiColors.Black}{timeSpacer}";
            var prefixText = $"{backgroundColor}{prefix}:{AnsiColors.Reset}";
            var rawPrefixText = $"{time}{timeSpacer}{prefix}: "; // can't use prefixText&timeText for length of text due to AnsiColors coloring

            // we don't handle ListLogType.Direct here because it's handled earlier in HandleList so the proper types can be logged instead of a string list
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
            var result = ListLogType switch
            {
                ListLogType.Multiple => GetResult
                (
                    string.Empty,
                    string.Empty.PadLeft(rawPrefixText.Length),
                    string.Empty,
                    Environment.NewLine
                ),
                ListLogType.Single => GetResult
                (
                    "[",
                    ",",
                    "]",
                    string.Empty
                ),
                _ => throw new ArgumentOutOfRangeException()
            };

            Console.Write($"{timeText}{prefixText} {color}{result}{AnsiColors.Reset}{Environment.NewLine}");

            return;

            string GetResult(string firstPos, string firstNeg, string lastPos, string lastNeg)
            {
                var finalResult = string.Empty;

                for (var i = 0; i < list.Count; i++)
                {
                    var first = i == 0;
                    var last = i == list.Count - 1;
                    var item = list[i].TrimStart();

                    if (!string.IsNullOrWhiteSpace(item))
                        finalResult += $"{(first ? firstPos : firstNeg)}{item}{(last ? lastPos : lastNeg)}";
                }

                return finalResult;
            }
        }
    }
}