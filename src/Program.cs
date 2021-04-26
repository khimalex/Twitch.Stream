using System;
using System.Diagnostics;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Twitch.Stream.Commands;
using Twitch.Stream.Profiles;
using Twitch.Stream.Services.ApiTwitchTv;
using Twitch.Stream.Services.UsherTwitchTv;

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
            .ConfigureHostConfiguration(b =>
            {
               b.AddJsonFile("appsettings.json", true, true);
            })
            .ConfigureServices((context, services) =>
            {
               services.Configure<Appsettings>(context.Configuration);
               services.AddHttpClient<KrakenApiTwitchTv>();
               services.AddHttpClient<HelixApiTwitchTv>();
               services.AddHttpClient<UsherTwitchTv>();
               services.AddAutoMapper(c =>
               {
                  c.AddProfile<HelixTwitchAuthToTwitchAuthDtoProfile>();
                  c.AddProfile<KrakenTwitchAuthToTwitchAuthDtoProfile>();
                  c.AddProfile<KrakenUsersToUsersDtoProfile>();
                  c.AddProfile<KrakenUserToUserDtoProfile>();
                  c.AddProfile<KrakenVideosToVideosDtoProfile>();
                  c.AddProfile<KrakenVideoToVideoDtoProfile>();
                  c.AddProfile<KrakenChannelToChannelDtoProfile>();
                  c.AddProfile<KrakenFpsToFpsDtoProfile>();
                  c.AddProfile<KrakenPreviewToVPreviewDtoProfile>();
                  c.AddProfile<KrakenResolutionsToResolutionsDtoProfile>();
                  c.AddProfile<KrakenThumbnailsToThumbnailsDtoProfile>();
                  c.AddProfile<KrakenThumbnailToThumbnailDtoProfile>();

               });
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
