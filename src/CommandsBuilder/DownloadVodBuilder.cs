using System;
using System.Collections.Generic;
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

      [Option("-id|--id", "Идентификаторы видео которые нужно скачать (-id XXXXXX -id YYYYYYY)", CommandOptionType.MultipleValue)]
      //TODO: Разобраться как проверить поле или список на наличие значения и непустоту списка, пользуясь "конвенцией".
      //[OptionRequired]
      public List<String> Ids { get; set; } = new List<String>();

      public async Task<Int32> OnExecuteAsync(CancellationToken ct = default)
      {
         Int32 result = 0;
         try
         {
            if (Ids.Any())
            {
               var op = _serviceProvider.GetRequiredService<IOptions<Appsettings>>();
               op.Value.Streams = Ids.ToArray();
               var commandWorker = _serviceProvider.GetServices<IApp>().FirstOrDefault(c => c is DownloadVod);
               await commandWorker.RunAsync(ct);
            }
            else
            {
               throw new Exception("Не задано ни одного идентификатора видео для скачивания!");
            }

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
