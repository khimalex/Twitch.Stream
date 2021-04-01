using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitch.Stream.App;

namespace Twitch.Stream.CommandBuilders
{
   static class DownloadLastBuilder
   {
      public const String _commandName = @"DownloadLast";
      public static void Build(IHost host, CommandLineApplication app)
      {
         app.Command(_commandName, command =>
         {
            command.Description = @"Скачать последний вод";
            command.HelpOption("-?|-h|--help");

            var nameOption = command.Option("-n|--name", "Имя стримера, последний вод которого нужно скачать", CommandOptionType.SingleValue);

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
                  var commandWorker = scope.ServiceProvider.GetServices<IApp>().FirstOrDefault(c => c is DownloadLast);
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
