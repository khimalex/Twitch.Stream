namespace Twitch.Stream.Commands.DownloadLast;

public class Request : IRequest<Response>
{
    public string ChannelName { get; set; }
}