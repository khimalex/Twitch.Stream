using Twitch.Stream.Commands.DownloadLast;

namespace Twitch.Stream.CommandsArgs;

[Command(_commandName, Description = @"Скачать последнюю запись стрима")]
internal class DownloadLastArgs
{
    public const string _commandName = @"DownloadLast";
    private readonly IMediator _mediator;

    public DownloadLastArgs(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Required]
    [Option("-n|--name", "Название канала последнее видео которого нужно скачать", CommandOptionType.SingleValue)]
    public string Last { get; set; }

    public async Task<int> OnExecuteAsync(CancellationToken ct = default)
    {
        _ = await _mediator.Send(new Request()
        {
            ChannelName = Last
        }, ct);

        return 0;
    }

}