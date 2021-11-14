using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands.DownloadLast;

internal class Handler : IRequestHandler<Request, Response>
{
    private readonly ILogger _logger;
    private readonly IApiTwitchTv _apiTwitch;
    private readonly IUsherTwitchTv _usherTwitch;
    private readonly Appsettings _options;

    public Handler(ILogger<Handler> logger, IOptions<Appsettings> optionsAccessor, IApiTwitchTv apiTwitch, IUsherTwitchTv usherTwitch)
    {
        _logger = logger;
        _apiTwitch = apiTwitch;
        _usherTwitch = usherTwitch;
        _options = optionsAccessor.Value;
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        string channelName = request.ChannelName;

        VideosDto videos = await _apiTwitch.GetChannelVideosAsync(channelName, 100);

        VideoDto lastVideo = videos.VideoList.FirstOrDefault();
        if (lastVideo is null)
        {
            throw new Exception($"Не найдено последнего видео стримера '{channelName}'.");
        }

        TwitchAuthDto vodTwitchAuth = await _apiTwitch.GetVodTwitchAuthAsync(lastVideo.Id);

        string invalidFileName = $@"{lastVideo.Type} {lastVideo.UserLogin} {lastVideo.Game} {lastVideo.CreatedAt.ToLocalTime()}.m3u8";
        string validFileName = string.Join(" ", invalidFileName.Split(Path.GetInvalidFileNameChars()));

        var query = new GetVodQueryParams { Sig = vodTwitchAuth.Sig, Token = vodTwitchAuth.Token };

        HttpContent httpContent = await _usherTwitch.GetVodAsync(lastVideo.Id, query);

        byte[] bytes = await httpContent.ReadAsByteArrayAsync(cancellationToken);

        await File.WriteAllBytesAsync(validFileName, bytes, cancellationToken);

        return new Response()
        {
            FileName = validFileName
        };
    }
}
