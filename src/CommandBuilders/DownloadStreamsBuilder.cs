using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitch.Stream.App;

namespace Twitch.Stream.CommandBuilders
{
   static class DownloadStreamsBuilder
   {
      public const String _commandName = @"DownloadStreams";
      public static void Build(IHost host, CommandLineApplication app)
      {
         app.Command(_commandName, command =>
         {
            command.Description = @"Скачать стримы из файла appsettings.json";
            command.HelpOption("-?|-h|--help");

            var nameOption = command.Option("-n|--name", "Скачать заданный стрим", CommandOptionType.SingleValue);

            command.OnExecuteAsync(async ct =>
            {
               using var scope = host.Services.CreateScope();
               try
               {
                  if (nameOption.HasValue())
                  {
                     var op = scope.ServiceProvider.GetRequiredService<IOptions<Appsettings>>();
                     op.Value.Streams = new[] { nameOption.Value() };
                  }
                  var commandWorker = scope.ServiceProvider.GetServices<IApp>().FirstOrDefault(c => c is DownloadStreams);
                  await commandWorker.RunAsync(ct);
               }
               catch (Exception e)
               {

                  scope.ServiceProvider.GetRequiredService<ILogger<Program>>().LogError(e, "Произошла ошибка работы приложения!");

               }
               return 0;
            });



         });

      }
   }
}
