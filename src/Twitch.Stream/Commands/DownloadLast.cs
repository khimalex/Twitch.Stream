using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitch.Libs.API;
using Twitch.Libs.API.Usher;
using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands
{
    internal class DownloadLast : IApp
    {
        private readonly ILogger _logger;
        private readonly IApiTwitchTv _apiTwitch;
        private readonly IUsherTwitchTv _usherTwitch;
        private readonly Appsettings _options;

        public DownloadLast(ILogger<DownloadInfo> logger, IOptions<Appsettings> optionsAccessor, IApiTwitchTv apiTwitch, IUsherTwitchTv usherTwitch)
        {
            _logger = logger;
            _apiTwitch = apiTwitch;
            _usherTwitch = usherTwitch;
            _options = optionsAccessor.Value;
        }

        public async Task RunAsync(CancellationToken token = default)
        {
            string channelName = _options.Streams.First();

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

            byte[] bytes = await httpContent.ReadAsByteArrayAsync(token);

            await File.WriteAllBytesAsync(validFileName, bytes, token);
            _logger.LogInformation(@"Последнее видео канала '{channelName}' '{filename}' загружено", channelName, validFileName);
        }
    }
}
