namespace Twitch.Stream.Commands.DownloadVod;

public class Request : IRequest<Response>
{
    public string VodId { get; set; }
}