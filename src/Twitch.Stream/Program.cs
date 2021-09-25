using System;
using System.Diagnostics;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using Twitch.Libs;
using Twitch.Libs.API;
using Twitch.Libs.API.Helix;
using Twitch.Libs.API.Usher;
using Twitch.Stream.Commands;
using static Twitch.Stream.Startup;

namespace Twitch.Stream
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        private static async Task MainAsync(string[] args)
        {

            var sw = new Stopwatch();
            sw.Start();
            IHostBuilder builder = Host.CreateDefaultBuilder().UseConsoleLifetime()
               .ConfigureAppConfiguration((c, b) =>
               {
#if DEBUG
                   //b.AddJsonFile($@"appsettings.{c.HostingEnvironment.EnvironmentName}.json", false, true);
#endif
               })
               .ConfigureServices((context, services) =>
               {
                   services.Configure<Appsettings>(context.Configuration);
                   services.ConfigureTwitchLibs(context.Configuration);

                   //services.AddHttpClient<KrakenApiTwitchTv>();
                   services.AddHttpClient<IApiTwitchTv, HelixApiTwitchTv>();
                   services.AddRefitClient<IUsherTwitchTv>()
                           .ConfigureHttpClient((sp, c) =>
                           {
                               UsherSettings usherSettings = sp.GetRequiredService<IOptions<UsherSettings>>().Value;
                               c.BaseAddress = new Uri(@"http://usher.twitch.tv");
                               c.DefaultRequestHeaders.TryAddWithoutValidation("Client-ID", usherSettings.ClientIDWeb);
                           });
                   //services.AddHttpClient<UsherTwitchTv>();

                   //todo: какую-нибудь бы фабричку
                   services.AddScoped<IApp, DownloadStreams>();
                   services.AddScoped<IApp, DownloadInfo>();
                   services.AddScoped<IApp, DownloadVod>();
                   services.AddScoped<IApp, DownloadLast>();
               })
               .ConfigureLogging(ConfigureLogging);

            CommandLineApplication<CommandsBuilder.ShortcutsBuilder> app = null;
            await builder.RunCommandLineApplicationAsync<CommandsBuilder.ShortcutsBuilder>(args, c => { app = c; });
            app?.ShowHelp();

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}
