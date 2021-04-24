using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitch.Stream.Commands;

namespace Twitch.Stream.CommandsBuilder
{
   [Command(DownloadInfoBuilder._commandName, Description = @"Показать информацию канала")]
   internal class DownloadInfoBuilder
   {
      public const String _commandName = @"DownloadInfo";
      private readonly IServiceProvider _serviceProvider;

      public DownloadInfoBuilder(IServiceProvider serviceProvider)
      {
         _serviceProvider = serviceProvider;
      }

      [Option("-n|--name", "Название канала последние видео которого нужно вывести", CommandOptionType.SingleValue)]
      public String Info { get; set; }

      public async Task<Int32> OnExecuteAsync(CancellationToken ct = default)
      {
         Int32 result = 0;
         try
         {
            if (!String.IsNullOrEmpty(Info))
            {
               var op = _serviceProvider.GetRequiredService<IOptions<Appsettings>>();
               op.Value.Streams = new[] { Info };

               var commandWorker = _serviceProvider.GetServices<IApp>().FirstOrDefault(c => c is DownloadInfo);
               await commandWorker.RunAsync(ct);
            }
            else
            {
               throw new Exception("Не задано название канала для получения информации!");
            }
         }
         catch (Exception e)
         {
            _serviceProvider.GetRequiredService<ILogger<DownloadInfoBuilder>>().LogError(e, "Произошла ошибка получения информации о канале!");
            result = 1;
         }
         return result;
      }

   }
}
