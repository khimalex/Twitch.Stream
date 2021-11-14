namespace Twitch.Stream.Commands.DownloadStreams;

public class SuccessHandler : IPipelineBehavior<Request, Response>
{
    private const string _message = @"Стрим '{ChannelName}' загружен в файл '{FileName}'";

    private readonly ILogger<SuccessHandler> _logger;

    public SuccessHandler(ILogger<SuccessHandler> logger)
    {
        _logger = logger;
    }

    public async Task<Response> Handle(Request request,
                                       CancellationToken cancellationToken,
                                       RequestHandlerDelegate<Response> next)
    {
        Response response = await next();

        _logger.LogInformation(_message, request.ChannelName, response.FileName);

        return response;
    }

}