using Twitch.Stream.Commands.DownloadInfo;

namespace Twitch.Stream.CommandsArgs;

[Command(_commandName, Description = @"Показать информацию канала")]
internal class DownloadInfoArgs
{
    public const string _commandName = @"DownloadInfo";
    private readonly IMediator _mediator;

    public DownloadInfoArgs(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Required]
    [Option("-n|--name", "Название канала последние видео которого нужно вывести", CommandOptionType.SingleValue)]
    public string Info { get; set; }

    public async Task<int> OnExecuteAsync(CancellationToken ct = default)
    {
        _ = await _mediator.Send(new Request()
        {
            ChannelName = Info
        }, ct);

        return 0;
    }
}