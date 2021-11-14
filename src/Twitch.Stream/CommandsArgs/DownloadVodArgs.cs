using Twitch.Stream.Commands.DownloadVod;

namespace Twitch.Stream.CommandsArgs;
[Command(_commandName, Description = @"Скачать видео")]
internal class DownloadVodBuilder
{
    public const string _commandName = @"DownloadVod";
    private readonly IMediator _mediator;

    public DownloadVodBuilder(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Required]
    [Option("-id|--id", "Идентификатор видео котороеые нужно скачать (-id XXXXXX)", CommandOptionType.SingleValue)]
    public string Id { get; set; }

    public async Task<int> OnExecuteAsync(CancellationToken ct = default)
    {
        _ = await _mediator.Send(new Request()
        {
            VodId = Id
        }, ct);

        return 0;
    }
}
