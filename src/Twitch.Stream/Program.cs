using System;
using System.Diagnostics;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Twitch.Libs;
using Twitch.Libs.API;
using Twitch.Libs.API.Helix;
using Twitch.Libs.API.Usher;
using Twitch.Stream.Commands;

namespace Twitch.Stream
{
    internal class Program
    {
        private static void Main(String[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        private static async Task MainAsync(String[] args)
        {

            var sw = new Stopwatch();
            sw.Start();
            IHostBuilder builder = Host.CreateDefaultBuilder().UseConsoleLifetime()
               .ConfigureAppConfiguration((c, b) =>
               {
#if DEBUG
                   b.AddJsonFile($@"appsettings.{c.HostingEnvironment.EnvironmentName}.json", false, true);
#endif
               })
               .ConfigureServices((context, services) =>
               {
                   services.Configure<Appsettings>(context.Configuration);
                   services.ConfigureTwitchLibs(context.Configuration);

                   //services.AddHttpClient<KrakenApiTwitchTv>();
                   services.AddHttpClient<IApiTwitchTv, HelixApiTwitchTv>();
                   services.AddHttpClient<UsherTwitchTv>();

                   //todo: какую-нибудь бы фабричку
                   services.AddScoped<IApp, DownloadStreams>();
                   services.AddScoped<IApp, DownloadInfo>();
                   services.AddScoped<IApp, DownloadVod>();
                   services.AddScoped<IApp, DownloadLast>();
               })
               .ConfigureLogging(loggerBuilder =>
               {
                   loggerBuilder.ClearProviders()
                   .AddFilter("System", LogLevel.None)
                   .AddFilter("Microsoft", LogLevel.None)
                   .AddFilter("Twitch.Stream", LogLevel.Information)
                   .AddSimpleConsole(c =>
                   {
                       c.IncludeScopes = true;
                   });
               });

            CommandLineApplication<CommandsBuilder.ShortcutsBuilder> app = null;
            await builder.RunCommandLineApplicationAsync<CommandsBuilder.ShortcutsBuilder>(args, c => { app = c; });
            app?.ShowHelp();

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}
