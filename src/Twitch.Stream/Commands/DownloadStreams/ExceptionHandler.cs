namespace Twitch.Stream.Commands.DownloadStreams;

public class ExceptionHandler : AsyncRequestExceptionHandler<Request, Response>
{
    private const string _message = "Ошибка скачивания стрима '{channelName}': {message}";
    private readonly ILogger<Request> _logger;

    public ExceptionHandler(ILogger<Request> logger)
    {
        _logger = logger;
    }
    protected override Task Handle(Request request,
                                   Exception exception,
                                   RequestExceptionHandlerState<Response> state,
                                   CancellationToken cancellationToken)
    {
        _logger.LogError(_message, request.ChannelName, exception.Message);

        state.SetHandled(default);
        return Task.CompletedTask;
    }
}