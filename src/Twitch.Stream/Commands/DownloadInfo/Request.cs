namespace Twitch.Stream.Commands.DownloadInfo;

public class Request : IRequest<Response>
{
    public string ChannelName { get; set; }
}