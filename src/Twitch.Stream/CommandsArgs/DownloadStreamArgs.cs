using Twitch.Stream.Commands;

namespace Twitch.Stream.CommandsArgs;

[Command(_commandName, Description = @"Скачать стримы из файла appsettings.json")]
internal class DownloadStreamArgs
{
    public const string _commandName = @"DownloadStreams";
    private readonly IMediator _mediator;

    public DownloadStreamArgs(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Option("-n|--name", "Скачать заданный стрим", CommandOptionType.SingleValue)]
    public string Name { get; set; }

    public async Task<int> OnExecuteAsync(CancellationToken ct = default)
    {
        _ = await _mediator.Send(new Commands.DownloadStreams.Request()
        {
            ChannelName = Name
        }, ct);

        return 0;
    }

}