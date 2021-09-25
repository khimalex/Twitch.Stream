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
    [Command(DownloadStreamsBuilder._commandName, Description = @"Скачать стримы из файла appsettings.json")]
    internal class DownloadStreamsBuilder
    {
        public const string _commandName = @"DownloadStreams";
        private readonly IServiceProvider _serviceProvider;

        public DownloadStreamsBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [Option("-n|--name", "Скачать заданный стрим", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        //Not necessary parameter `CommandLineApplication`
        //public async Task<Int32> OnExecuteAsync(CommandLineApplication app, CancellationToken ct = default)
        public async Task<int> OnExecuteAsync(CancellationToken ct = default)
        {
            int result = 0;
            try
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    IOptions<Appsettings> op = _serviceProvider.GetRequiredService<IOptions<Appsettings>>();
                    op.Value.Streams = new[] { Name };
                }

                IApp commandWorker = _serviceProvider.GetServices<IApp>().FirstOrDefault(c => c is DownloadStreams);
                await commandWorker.RunAsync(ct);
            }
            catch (Exception e)
            {
                _serviceProvider.GetRequiredService<ILogger<DownloadStreamsBuilder>>().LogError(e, "Произошла ошибка скачивания стримов!");
                result = 1;
            }
            
            return result;
        }

    }
}
