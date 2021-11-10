namespace Twitch.Stream.CommandsBuilder;

[Subcommand(typeof(DownloadStreamsBuilder), typeof(DownloadInfoBuilder), typeof(DownloadLastBuilder), typeof(DownloadVodBuilder))]
internal class ShortcutsBuilder
{
    private readonly IServiceProvider _serviceProvider;

    public ShortcutsBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [Option(@"-n|--name", "Скачать конкретный стрим", CommandOptionType.SingleValue)]
    public string Name { get; set; }

    [Option("-i|--info", "Получить информацию по последним видео канала", CommandOptionType.SingleValue)]
    public string Info { get; set; }

    [Option("-l|--last", "Скачать последнее по дате видео канала", CommandOptionType.SingleValue)]
    public string Last { get; set; }

    [Option("-d|--download", "Скачать видео из списка", CommandOptionType.MultipleValue)]
    public List<string> Ids { get; set; } = new List<string>();

    //Not necessary parameter `CommandLineApplication`
    //public async Task<Int32> OnExecuteAsync(CommandLineApplication app, CancellationToken ct = default)
    public async Task<int> OnExecuteAsync(CancellationToken ct = default)
    {

        var commandsTasks = new List<Task>();

        if (!string.IsNullOrWhiteSpace(Name))
        {
            commandsTasks.Add(new DownloadStreamsBuilder(_serviceProvider.CreateScope().ServiceProvider) { Name = Name }.OnExecuteAsync(ct));
        }

        if (!string.IsNullOrWhiteSpace(Info))
        {
            commandsTasks.Add(new DownloadInfoBuilder(_serviceProvider.CreateScope().ServiceProvider) { Info = Info }.OnExecuteAsync(ct));
        }

        if (Ids is not null && Ids.Any())
        {
            commandsTasks.Add(new DownloadVodBuilder(_serviceProvider.CreateScope().ServiceProvider) { Ids = Ids }.OnExecuteAsync(ct));
        }

        if (!string.IsNullOrWhiteSpace(Last))
        {
            commandsTasks.Add(new DownloadLastBuilder(_serviceProvider.CreateScope().ServiceProvider) { Last = Last }.OnExecuteAsync(ct));
        }

        if (commandsTasks is { Count: 0 })
        {
            commandsTasks.Add(new DownloadStreamsBuilder(_serviceProvider.CreateScope().ServiceProvider).OnExecuteAsync(ct));
        }

        await Task.WhenAll(commandsTasks);
        return 0;

    }
}
