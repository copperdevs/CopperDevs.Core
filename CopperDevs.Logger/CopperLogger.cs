﻿using System;

namespace CopperDevs.Logger
{
    /// <summary>
    /// Core logger class
    /// </summary>
    public static class CopperLogger
    {
        /// <summary>
        /// Setting for if the time should be added to all logged messages
        /// </summary>
        public static bool IncludeTimestamps = true;

        public static void Log(object message, CustomLog log)
        {
            LogMessage(log.MainColor, log.BackgroundColor, log.Prefix, message);
        }

        internal static void LogMessage(AnsiColors.Names colorName, string prefix, object message)
        {
            LogMessage(colorName, colorName, prefix, message);
        }

        private static void LogMessage(AnsiColors.Names colorName, AnsiColors.Names backgroundColorName, string prefix, object message)
        {
            var color = AnsiColors.GetColor(colorName);
            var backgroundColor = AnsiColors.GetBackgroundColor(backgroundColorName);

            var time = IncludeTimestamps ? $"{DateTime.Now:HH:mm:ss}" : "";
            var timeSpacer = IncludeTimestamps ? " " : "";

            Console.Write($"{AnsiColors.Black}{AnsiColors.LightGrayBackground}{time}{AnsiColors.Reset}{AnsiColors.Black}{timeSpacer}{backgroundColor}{prefix}:{AnsiColors.Reset} {color}{message}{AnsiColors.Reset}{Environment.NewLine}");
        }
    }
}