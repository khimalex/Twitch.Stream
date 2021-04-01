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
   class DownloadVodBuilder
   {
      public const String _commandName = @"DownloadVod";

      public static void Build(IHost host, CommandLineApplication app)
      {
         app.Command(_commandName, command =>
         {
            command.Description = @"Скачать воды";
            command.HelpOption("-?|-h|--help");


            var idOption = command.Option("-id|--id", "Идентификатор вода которе нужно скачать", CommandOptionType.MultipleValue, co => co.IsRequired(errorMessage: @"Не заданы идентификаторы скачиваемых видео!"));

            command.OnExecuteAsync(async ct =>
            {
               using var scope = host.Services.CreateScope();

               try
               {
                  if (idOption.HasValue())
                  {
                     var op = scope.ServiceProvider.GetRequiredService<IOptions<Appsettings>>();

                     op.Value.Streams = idOption.Values.ToArray();
                  }
                  var commandWorker = scope.ServiceProvider.GetServices<IApp>().FirstOrDefault(c => c is DownloadVod);
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
