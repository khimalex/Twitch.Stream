using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands.DownloadInfo;

public class SuccessHandler : IPipelineBehavior<Request, Response>
{
    private const string _message = "Последнее видео канала '{ChannelName}' '{FileName}' загружено";

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

        foreach (VideoDto item in response.Videos.VideoList)
        {
            Console.WriteLine($@"{"channel",15}: {item.UserLogin}");
            Console.WriteLine($@"{"id",15}: {item.Id}");
            Console.WriteLine($@"{"type",15}: {item.Type}");
            Console.WriteLine($@"{"title",15}: {item.Title}");
            Console.WriteLine($@"{"game",15}: {item.Game}");
            Console.WriteLine($@"{"DateTime",15}: {item.CreatedAt.ToLocalTime()}");
            Console.WriteLine(new string('-', Console.WindowWidth));
        }

        return response;
    }
}