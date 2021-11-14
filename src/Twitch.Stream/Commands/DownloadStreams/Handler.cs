using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands.DownloadStreams;

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
        string user = request.ChannelName;
        string fileName = $"{user}{PreHandler._extension}";

        TwitchAuthDto twitchauth = await _apiTwitch.GetChannelTwitchAuthAsync(user);

        var query = new GetStreamQueryParams
        {
            Sig = twitchauth.Sig,
            Token = twitchauth.Token
        };

        HttpContent httpContent = await _usherTwitch.GetStreamAsync(user, query);

        byte[] bytes = await httpContent.ReadAsByteArrayAsync(cancellationToken);

        await File.WriteAllBytesAsync(fileName, bytes, cancellationToken);

        return new Response()
        {
            ChannelName = user,
            FileName = fileName
        };
    }

}