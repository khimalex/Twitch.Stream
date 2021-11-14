using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands.DownloadInfo;

internal class Handler : IRequestHandler<Request, Response>
{
    private readonly IApiTwitchTv _apiTwitch;

    public Handler(IApiTwitchTv apiTwitch)
    {
        _apiTwitch = apiTwitch;
    }

    public async Task<Response> Handle(Request notification,
                                       CancellationToken cancellationToken)
    {
        VideosDto videos = await _apiTwitch.GetChannelVideosAsync(notification.ChannelName, 100);
        videos.VideoList.Reverse();

        return new Response()
        {
            Videos = videos
        };
    }
}
