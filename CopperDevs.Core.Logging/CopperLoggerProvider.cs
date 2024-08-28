using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CopperDevs.Core.Logging;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("CopperLogger")]
public sealed class CopperLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? onChangeToken;
    private CopperLoggerConfiguration currentConfig;
    private readonly ConcurrentDictionary<string, CopperLogger> loggers = new(StringComparer.OrdinalIgnoreCase);

    public CopperLoggerProvider(IOptionsMonitor<CopperLoggerConfiguration> config)
    {
        currentConfig = config.CurrentValue;
        onChangeToken = config.OnChange(updatedConfig => currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName) => loggers.GetOrAdd(categoryName, name => new CopperLogger(name, GetCurrentConfig));

    private CopperLoggerConfiguration GetCurrentConfig() => currentConfig;

    public void Dispose()
    {
        loggers.Clear();
        onChangeToken?.Dispose();
    }
}