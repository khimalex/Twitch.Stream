using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Twitch.Stream.CommandBuilders;
using Twitch.Stream.Profiles;
using Twitch.Stream.Services.ApiTwitchTv;
using Twitch.Stream.Services.UsherTwitchTv;

namespace Twitch.Stream
{
   class Program
   {
      static async Task Main(String[] args)
      {

         var builder = Host.CreateDefaultBuilder().UseConsoleLifetime()
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
               services.AddScoped<App.IApp, App.DownloadStreams>();
               services.AddScoped<App.IApp, App.DownloadInfo>();
               services.AddScoped<App.IApp, App.DownloadVod>();
               services.AddScoped<App.IApp, App.DownloadLast>();
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
         var host = builder.Build();

         var app = new CommandLineApplication
         {
            Name = @"Twitch.Streams",
            Description = @"Загрузка стримов и водов"
         };

         var nameOption = app.Option("-n|--name", "Скачать конкретный стрим", CommandOptionType.SingleValue);
         var infoOption = app.Option("-i|--info", "Получить информацию по последним видео для канала", CommandOptionType.SingleValue);
         var idOptions = app.Option("-d|--download", "Скачать воды", CommandOptionType.MultipleValue);
         var lastOption = app.Option("-l|--last", "Скачать последний по дате вод", CommandOptionType.SingleValue);

         app.HelpOption("-?|-h|--help|--памагите");

         app.OnExecuteAsync(async ct =>
         {
            var commandsTasks = new List<Task>();
            Boolean anyLaunched = false;
            if (nameOption.HasValue())
            {
               anyLaunched = true;
               commandsTasks.Add(app.ExecuteAsync(new[] { DownloadStreamsBuilder._commandName, "-n", nameOption.Value() }, ct));

            }
            if (infoOption.HasValue())
            {
               anyLaunched = true;
               commandsTasks.Add(app.ExecuteAsync(new[] { DownloadInfoBuilder._commandName, "-n", infoOption.Value() }, ct));
            }
            if (idOptions.HasValue())
            {
               anyLaunched = true;
               var rebuildedArgs = new List<String> { DownloadVodBuilder._commandName };
               foreach (String item in idOptions.Values)
               {
                  rebuildedArgs.Add("-id");
                  rebuildedArgs.Add(item);
               }
               commandsTasks.Add(app.ExecuteAsync(rebuildedArgs.ToArray(), ct));

            }
            if (lastOption.HasValue())
            {
               anyLaunched = true;
               commandsTasks.Add(app.ExecuteAsync(new[] { DownloadLastBuilder._commandName, "-n", lastOption.Value() }, ct));


            }
            if (!anyLaunched)
            {
               commandsTasks.Add(app.ExecuteAsync(new[] { DownloadStreamsBuilder._commandName }, ct));
            }

            await Task.WhenAll(commandsTasks);
            return 0;
         });

         DownloadStreamsBuilder.Build(host, app);
         DownloadInfoBuilder.Build(host, app);
         DownloadVodBuilder.Build(host, app);
         DownloadLastBuilder.Build(host, app);

         var sw = new Stopwatch();
         sw.Start();

         await app.ExecuteAsync(args);

         if (host is IDisposable disposable)
         {
            disposable.Dispose();
         }

         sw.Stop();
         Console.WriteLine(sw.Elapsed);
      }
   }
}
