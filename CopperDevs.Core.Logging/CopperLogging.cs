using Microsoft.Extensions.Logging;

namespace CopperDevs.Core.Logging;

public sealed class CopperLogging(string name, Func<CopperLoggingConfiguration> getCurrentConfig) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => getCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var config = getCurrentConfig();

        if (config.EventId != 0 && config.EventId != eventId.Id)
            return;

        config.LogLevelToColorMap[logLevel].Invoke($"{name} - {formatter(state, exception)}");
    }
}