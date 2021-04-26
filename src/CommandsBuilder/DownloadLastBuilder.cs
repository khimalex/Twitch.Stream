using System;
using System.ComponentModel.DataAnnotations;
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
   [Command(DownloadLastBuilder._commandName, Description = @"Скачать последнюю запись стрима")]
   internal class DownloadLastBuilder
   {
      public const String _commandName = @"DownloadLast";
      private readonly IServiceProvider _serviceProvider;

      public DownloadLastBuilder(IServiceProvider serviceProvider)
      {
         _serviceProvider = serviceProvider;
      }

      [Required]
      [Option("-n|--name", "Название канала последнее видео которого нужно скачать", CommandOptionType.SingleValue)]
      public String Last { get; set; }

      //Not necessary parameter `CommandLineApplication`
      //public async Task<Int32> OnExecuteAsync(CommandLineApplication app, CancellationToken ct = default)
      public async Task<Int32> OnExecuteAsync(CancellationToken ct = default)
      {
         Int32 result = 0;
         try
         {
            if (!String.IsNullOrEmpty(Last))
            {
               IOptions<Appsettings> op = _serviceProvider.GetRequiredService<IOptions<Appsettings>>();
               op.Value.Streams = new[] { Last };

               IApp commandWorker = _serviceProvider.GetServices<IApp>().FirstOrDefault(c => c is DownloadLast);
               await commandWorker.RunAsync(ct);
            }
            else
            {
               throw new Exception("Не задано название канала для скачивания последнего видео!");
            }

         }
         catch (Exception e)
         {
            _serviceProvider.GetRequiredService<ILogger<DownloadLastBuilder>>().LogError(e, "Произошла ошибка получения информации о канале!");
            result = 1;
         }
         return result;
      }

   }
}
