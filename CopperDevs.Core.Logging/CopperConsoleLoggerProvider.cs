using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CopperDevs.Core.Logging;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("CopperConsole")]
public sealed class CopperConsoleLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? onChangeToken;
    private CopperConsoleLoggerConfiguration currentConfig;
    private readonly ConcurrentDictionary<string, CopperConsoleLogger> loggers = new(StringComparer.OrdinalIgnoreCase);

    public CopperConsoleLoggerProvider(IOptionsMonitor<CopperConsoleLoggerConfiguration> config)
    {
        currentConfig = config.CurrentValue;
        onChangeToken = config.OnChange(updatedConfig => currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName) => loggers.GetOrAdd(categoryName, name => new CopperConsoleLogger(name, GetCurrentConfig));

    private CopperConsoleLoggerConfiguration GetCurrentConfig() => currentConfig;

    public void Dispose()
    {
        loggers.Clear();
        onChangeToken?.Dispose();
    }
}