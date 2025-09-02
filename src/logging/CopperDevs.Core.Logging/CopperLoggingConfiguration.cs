using CopperDevs.Logger;
using Microsoft.Extensions.Logging;

namespace CopperDevs.Core.Logging;

public sealed class CopperLoggingConfiguration
{
    public int EventId { get; init; }

    public Dictionary<LogLevel, Action<object>> LogLevelToColorMap { get; init; } = new()
    {
        [LogLevel.Trace] = Log.Trace,
        [LogLevel.Debug] = Log.Debug,
        [LogLevel.Warning] = Log.Warn,
        [LogLevel.Information] = Log.Info,
        [LogLevel.Error] = Log.Error,
        [LogLevel.Critical] = Log.Critical
    };
}