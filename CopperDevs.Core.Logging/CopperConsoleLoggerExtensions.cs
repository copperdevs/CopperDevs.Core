using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace CopperDevs.Core.Logging;

public static class CopperConsoleLoggerExtensions
{
    public static ILoggingBuilder AddCopperConsoleLogger(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, CopperConsoleLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions<CopperConsoleLoggerConfiguration, CopperConsoleLoggerProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddCopperConsoleLogger(this ILoggingBuilder builder, Action<CopperConsoleLoggerConfiguration> configure)
    {
        builder.AddCopperConsoleLogger();
        builder.Services.Configure(configure);

        return builder;
    }
}