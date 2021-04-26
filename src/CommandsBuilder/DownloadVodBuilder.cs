using System;
using System.Collections.Generic;
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
   [Command(DownloadVodBuilder._commandName, Description = @"Скачать видео")]
   internal class DownloadVodBuilder
   {
      public const String _commandName = @"DownloadVod";
      private readonly IServiceProvider _serviceProvider;

      public DownloadVodBuilder(IServiceProvider serviceProvider)
      {
         _serviceProvider = serviceProvider;
      }

      [Required]
      [Option("-id|--id", "Идентификаторы видео которые нужно скачать (-id XXXXXX -id YYYYYYY)", CommandOptionType.MultipleValue)]
      public List<String> Ids { get; set; } = new List<String>();

      //Not necessary parameter `CommandLineApplication`
      //public async Task<Int32> OnExecuteAsync(CommandLineApplication app, CancellationToken ct = default)
      public async Task<Int32> OnExecuteAsync(CancellationToken ct = default)
      {
         Int32 result = 0;
         try
         {
            IOptions<Appsettings> op = _serviceProvider.GetRequiredService<IOptions<Appsettings>>();
            op.Value.Streams = Ids.ToArray();
            IApp commandWorker = _serviceProvider.GetServices<IApp>().FirstOrDefault(c => c is DownloadVod);
            await commandWorker.RunAsync(ct);
         }
         catch (Exception e)
         {
            _serviceProvider.GetRequiredService<ILogger<DownloadVodBuilder>>().LogError(e, "Произошла ошибка получения видео!");
            result = 1;
         }
         return result;
      }


   }
}
