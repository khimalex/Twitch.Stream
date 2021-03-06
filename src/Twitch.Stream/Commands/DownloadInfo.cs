using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitch.Libs.API;
using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands
{
    internal class DownloadInfo : IApp
    {
        private readonly ILogger _logger;
        private readonly IApiTwitchTv _apiTwitch;
        private readonly Appsettings _options;

        public DownloadInfo(ILogger<DownloadInfo> logger, IOptions<Appsettings> optionsAccessor, IApiTwitchTv apiTwitch)
        {
            _logger = logger;
            _apiTwitch = apiTwitch;
            _options = optionsAccessor.Value;
        }
        public async Task RunAsync(CancellationToken token = default)
        {
            String channelName = _options.Streams.First();

            VideosDto videos = await _apiTwitch.GetChannelVideosAsync(channelName, 100);
            videos.Videos.Reverse();
            foreach (VideoDto item in videos.Videos)
            {
                Console.WriteLine($@"{"channel",15}: {item.Login}");
                Console.WriteLine($@"{"id",15}: {item.Id}");
                Console.WriteLine($@"{"type",15}: {item.Type}");
                Console.WriteLine($@"{"title",15}: {item.Title}");
                Console.WriteLine($@"{"game",15}: {item.Game}");
                Console.WriteLine($@"{"DateTime",15}: {item.CreatedAt.ToLocalTime()}");
                Console.WriteLine(new String('-', Console.WindowWidth));
            }
        }
    }
}
