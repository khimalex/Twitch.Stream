using System;
using System.IO;
using System.Linq;
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
        private readonly UsherTwitchTv _usherTwitch;
        private readonly Appsettings _options;

        public DownloadLast(ILogger<DownloadInfo> logger, IOptions<Appsettings> optionsAccessor, IApiTwitchTv apiTwitch, UsherTwitchTv usherTwitch)
        {
            _logger = logger;
            _apiTwitch = apiTwitch;
            _usherTwitch = usherTwitch;
            _options = optionsAccessor.Value;
        }

        public async Task RunAsync(CancellationToken token = default)
        {
            String channelName = _options.Streams.First();

            VideosDto videos = await _apiTwitch.GetChannelVideosAsync(channelName, 100);

            VideoDto lastVideo = videos.Videos.FirstOrDefault();
            if (lastVideo is null)
            {
                throw new Exception($"Не найдено последнего видео стримера '{channelName}'.");
            }
            TwitchAuthDto vodTwitchAuth = await _apiTwitch.GetVodTwitchAuthAsync(lastVideo.Id);

            String invalidFileName = $@"{lastVideo.Type} {lastVideo.Login} {lastVideo.Game} {lastVideo.CreatedAt.ToLocalTime()}.m3u8";
            String validFileName = String.Join(" ", invalidFileName.Split(Path.GetInvalidFileNameChars()));

            Byte[] videoData = await _usherTwitch.GetVideoAsync(lastVideo.Id, vodTwitchAuth);

            await File.WriteAllBytesAsync(validFileName, videoData, token);
            _logger.LogInformation(@"Последнее видео канала '{channelName}' '{filename}' загружено", channelName, validFileName);


        }
    }
}
