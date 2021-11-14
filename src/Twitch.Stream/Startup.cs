using System.Reflection;

namespace Twitch.Stream;

internal static class Startup
{
    public static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder loggingBuilder)
    {
        _ = loggingBuilder.ClearProviders()
            .SetMinimumLevel(LogLevel.Trace)
            .AddNLog(GetLoggingConfiguration());

        LoggingConfiguration GetLoggingConfiguration()
        {
            LoggingConfiguration loggingConfiguration = new();

            loggingConfiguration.AddTarget("console", new ColoredConsoleTarget()
            {
                AutoFlush = true,
                UseDefaultRowHighlightingRules = true,
                Layout = Layout.FromString("${newline}${level:uppercase=true}: ${message:withexception=true}")

            });
            loggingConfiguration.AddTarget("blackhole", new NullTarget());

            loggingConfiguration.AddRule(NLog.LogLevel.Info,
                                         NLog.LogLevel.Fatal,
                                         "console", "Twitch.*");

            return loggingConfiguration;
        }

    }

    public static IServiceCollection AddMediatRAndHandlers(this IServiceCollection services)
    {
        return services.AddMediatR(Assembly.GetExecutingAssembly())
        .AddScoped(typeof(IPipelineBehavior<Commands.DownloadStreams.Request, Commands.DownloadStreams.Response>), typeof(Commands.DownloadStreams.SuccessHandler))
        .AddScoped(typeof(IPipelineBehavior<Commands.DownloadVod.Request, Commands.DownloadVod.Response>), typeof(Commands.DownloadVod.SuccessHandler))
        .AddScoped(typeof(IPipelineBehavior<Commands.DownloadInfo.Request, Commands.DownloadInfo.Response>), typeof(Commands.DownloadInfo.SuccessHandler))
        .AddScoped(typeof(IPipelineBehavior<Commands.DownloadLast.Request, Commands.DownloadLast.Response>), typeof(Commands.DownloadLast.SuccessHandler));
    }
}
