using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Twitch.Stream.CommandsBuilder
{
    [Subcommand(typeof(DownloadStreamsBuilder), typeof(DownloadInfoBuilder), typeof(DownloadLastBuilder), typeof(DownloadVodBuilder))]
    internal class ShortcutsBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public ShortcutsBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        [Option(@"-n|--name", "Скачать конкретный стрим", CommandOptionType.SingleValue)]
        public String Name { get; set; }

        [Option("-i|--info", "Получить информацию по последним видео канала", CommandOptionType.SingleValue)]
        public String Info { get; set; }

        [Option("-l|--last", "Скачать последнее по дате видео канала", CommandOptionType.SingleValue)]
        public String Last { get; set; }

        [Option("-d|--download", "Скачать видео из списка", CommandOptionType.MultipleValue)]
        public List<String> Ids { get; set; } = new List<String>();

        //Not necessary parameter `CommandLineApplication`
        //public async Task<Int32> OnExecuteAsync(CommandLineApplication app, CancellationToken ct = default)
        public async Task<Int32> OnExecuteAsync(CancellationToken ct = default)
        {

            var commandsTasks = new List<Task>();
            Boolean anyLaunched = false;
            if (!String.IsNullOrEmpty(Name))
            {
                anyLaunched = true;
                commandsTasks.Add(new DownloadStreamsBuilder(_serviceProvider.CreateScope().ServiceProvider) { Name = Name }.OnExecuteAsync(ct));
            }
            if (!String.IsNullOrEmpty(Info))
            {
                anyLaunched = true;
                commandsTasks.Add(new DownloadInfoBuilder(_serviceProvider.CreateScope().ServiceProvider) { Info = Info }.OnExecuteAsync(ct));
            }
            if (Ids is not null && Ids.Any())
            {
                anyLaunched = true;
                commandsTasks.Add(new DownloadVodBuilder(_serviceProvider.CreateScope().ServiceProvider) { Ids = Ids }.OnExecuteAsync(ct));
            }
            if (!String.IsNullOrEmpty(Last))
            {
                anyLaunched = true;
                commandsTasks.Add(new DownloadLastBuilder(_serviceProvider.CreateScope().ServiceProvider) { Last = Last }.OnExecuteAsync(ct));
            }
            if (!anyLaunched)
            {
                commandsTasks.Add(new DownloadStreamsBuilder(_serviceProvider.CreateScope().ServiceProvider).OnExecuteAsync(ct));
            }

            await Task.WhenAll(commandsTasks);
            return 0;

        }
    }
}
