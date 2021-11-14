namespace Twitch.Stream.Commands.DownloadVod;

public class SuccessHandler : IPipelineBehavior<Request, Response>
{
    private const string _message = "Видео '{videoId}' загружено в '{FileName}'.";
    private readonly ILogger<SuccessHandler> _logger;

    public SuccessHandler(ILogger<SuccessHandler> logger)
    {
        _logger = logger;
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken, RequestHandlerDelegate<Response> next)
    {
        Response response = await next();

        _logger.LogInformation(_message, response.VodId, response.FileName);

        return response;
    }
}