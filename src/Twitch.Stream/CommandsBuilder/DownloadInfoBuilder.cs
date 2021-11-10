namespace Twitch.Stream.CommandsBuilder;

[Command(DownloadInfoBuilder._commandName, Description = @"Показать информацию канала")]
internal class DownloadInfoBuilder
{
    public const string _commandName = @"DownloadInfo";
    private readonly IServiceProvider _serviceProvider;

    public DownloadInfoBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [Required]
    [Option("-n|--name", "Название канала последние видео которого нужно вывести", CommandOptionType.SingleValue)]
    public string Info { get; set; }

    //Not necessary parameter `CommandLineApplication`
    //public async Task<Int32> OnExecuteAsync(CommandLineApplication app, CancellationToken ct = default)

    public async Task<int> OnExecuteAsync(CancellationToken ct = default)
    {
        int result = 0;
        try
        {
            if (!string.IsNullOrEmpty(Info))
            {
                IOptions<Appsettings> op = _serviceProvider.GetRequiredService<IOptions<Appsettings>>();
                op.Value.Streams = new[] { Info };

                IApp commandWorker = _serviceProvider.GetServices<IApp>().FirstOrDefault(c => c is DownloadInfo);
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