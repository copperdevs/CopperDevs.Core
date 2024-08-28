using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CopperDevs.Core.Logging;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("CopperLogger")]
public sealed class CopperLoggingProvider : ILoggerProvider
{
    private readonly IDisposable? onChangeToken;
    private CopperLoggingConfiguration currentConfig;
    private readonly ConcurrentDictionary<string, CopperLogging> loggers = new(StringComparer.OrdinalIgnoreCase);

    public CopperLoggingProvider(IOptionsMonitor<CopperLoggingConfiguration> config)
    {
        currentConfig = config.CurrentValue;
        onChangeToken = config.OnChange(updatedConfig => currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName) => loggers.GetOrAdd(categoryName, name => new CopperLogging(name, GetCurrentConfig));

    private CopperLoggingConfiguration GetCurrentConfig() => currentConfig;

    public void Dispose()
    {
        loggers.Clear();
        onChangeToken?.Dispose();
    }
}