namespace Twitch.Stream.Commands.DownloadStreams;

public class Request : IRequest<Response>
{
    public string ChannelName { get; set; }
}