using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands.DownloadVod;

internal class Handler : IRequestHandler<Request, Response>
{
    private readonly IApiTwitchTv _apiTwitch;
    private readonly IUsherTwitchTv _usherTwitch;

    public Handler(IApiTwitchTv apiTwitch,
                   IUsherTwitchTv usherTwitch)
    {
        _apiTwitch = apiTwitch;
        _usherTwitch = usherTwitch;
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        string videoId = request.VodId;

        VideosDto videos = await _apiTwitch.GetVideoInfoAsync(videoId);
        if (videos.VideoList is { Count: 0 })
        {
            throw new Exception("Видео не найдено!");
        }

        VideoDto video = videos.VideoList[0];

        TwitchAuthDto vodTwitchAuth = await _apiTwitch.GetVodTwitchAuthAsync(videoId);

        string invalidFileName = $@"{video.Type} {video.UserLogin} {video.Game} {video.CreatedAt.ToLocalTime()}.m3u8";
        string validFileName = string.Join(" ", invalidFileName.Split(Path.GetInvalidFileNameChars()));

        var query = new GetVodQueryParams { Sig = vodTwitchAuth.Sig, Token = vodTwitchAuth.Token };

        HttpContent httpContent = await _usherTwitch.GetVodAsync(video.Id, query);

        byte[] bytes = await httpContent.ReadAsByteArrayAsync(cancellationToken);

        await File.WriteAllBytesAsync(validFileName, bytes, cancellationToken);

        return new Response()
        {
            VodId = request.VodId,
            FileName = validFileName
        };
    }
}