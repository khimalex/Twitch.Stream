namespace Twitch.Stream.CommandsArgs;

[Subcommand(typeof(DownloadStreamArgs),
            typeof(DownloadInfoArgs),
            typeof(DownloadLastArgs),
            typeof(DownloadVodBuilder))]
internal class ShortcutsBuilder
{
    private readonly IMediator _mediator;
    private readonly Appsettings _options;

    public ShortcutsBuilder(IMediator mediator, IOptions<Appsettings> optionsAccessor)
    {
        _mediator = mediator;
        _options = optionsAccessor.Value;
    }
    [Option(@"-n|--name", "Скачать конкретный стрим (-n name1 -n name2)", CommandOptionType.MultipleValue)]
    public List<string> Names { get; set; } = new List<string>();

    [Option("-i|--info", "Получить информацию по последним видео канала", CommandOptionType.SingleValue)]
    public string Info { get; set; }

    [Option("-l|--last", "Скачать последнее по дате видео канала (-l name -l name)", CommandOptionType.MultipleValue)]
    public List<string> Lasts { get; set; } = new List<string>();

    [Option("-d|--download", "Скачать видео из списка (-d videoId1 -d videoId2)", CommandOptionType.MultipleValue)]
    public List<string> DownloadIds { get; set; } = new List<string>();

    public async Task<int> OnExecuteAsync(CancellationToken ct = default)
    {
        var commandsTasks = new List<Task>();

        if (Names is { Count: > 0 })
        {
            foreach (string item in Names)
            {
                commandsTasks.Add(_mediator.Send(new Commands.DownloadStreams.Request()
                {
                    ChannelName = item
                }, ct));
            }
        }

        if (!string.IsNullOrWhiteSpace(Info))
        {
            commandsTasks.Add(_mediator.Send(new Commands.DownloadInfo.Request()
            {
                ChannelName = Info
            }, ct));
        }

        if (DownloadIds is { Count: > 0 })
        {
            foreach (string item in DownloadIds)
            {
                commandsTasks.Add(_mediator.Send(new Commands.DownloadVod.Request()
                {
                    VodId = item
                }, ct));
            }
        }

        if (Lasts is { Count: > 0 })
        {
            foreach (string item in Lasts)
            {
                commandsTasks.Add(_mediator.Send(new Commands.DownloadLast.Request()
                {
                    ChannelName = item
                }, ct));
            }
        }

        if (commandsTasks is { Count: 0 })
        {
            foreach (string item in _options.Streams)
            {
                commandsTasks.Add(_mediator.Send(new Commands.DownloadStreams.Request()
                {
                    ChannelName = item
                }, ct));
            }
        }

        await Task.WhenAll(commandsTasks);
        
        return 0;

    }
}
