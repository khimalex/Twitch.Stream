using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Layouts;
using NLog.Targets;

namespace Twitch.Stream
{
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
    }
}
