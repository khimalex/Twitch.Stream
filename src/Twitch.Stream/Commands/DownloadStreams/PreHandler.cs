namespace Twitch.Stream.Commands.DownloadStreams;

public class PreHandler : IRequestPreProcessor<Request>
{
    internal const string _extension = ".m3u8";
    public Task Process(Request request, CancellationToken cancellationToken)
    {
        string fileName = $"{request.ChannelName}{_extension}";

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        return Task.CompletedTask;
    }
}