using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace CopperDevs.Core.Logging;

public static class Extensions
{
    public static ILoggingBuilder AddCopperLogger(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, CopperLoggingProvider>());

        LoggerProviderOptions.RegisterProviderOptions<CopperLoggingConfiguration, CopperLoggingProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddCopperLogger(this ILoggingBuilder builder, Action<CopperLoggingConfiguration> configure)
    {
        builder.AddCopperLogger();
        builder.Services.Configure(configure);

        return builder;
    }
}