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
    internal class DownloadVod : IApp
    {
        private readonly Appsettings _options;
        private readonly ILogger _logger;
        private readonly IApiTwitchTv _apiTwitch;
        private readonly IUsherTwitchTv _usherTwitch;

        public DownloadVod(ILogger<DownloadVod> logger, IOptions<Appsettings> optionsAccessor, IApiTwitchTv apiTwitch, IUsherTwitchTv usherTwitch)
        {
            _options = optionsAccessor.Value;
            _logger = logger;
            _apiTwitch = apiTwitch;
            _usherTwitch = usherTwitch;
        }
        public async Task RunAsync(CancellationToken token = default)
        {
            System.Collections.Generic.IEnumerable<Task> tasks = _options.Streams.Select(async videoId =>
            {

                VideosDto videos = await _apiTwitch.GetVideoInfoAsync(videoId);
                if (!videos.VideoList.Any())
                {
                    throw new Exception($"Не найдено видео '{videoId}'.");

                }

                VideoDto video = videos.VideoList.First();

                TwitchAuthDto vodTwitchAuth = await _apiTwitch.GetVodTwitchAuthAsync(videoId);

                string invalidFileName = $@"{video.Type} {video.UserLogin} {video.Game} {video.CreatedAt.ToLocalTime()}.m3u8";
                string validFileName = string.Join(" ", invalidFileName.Split(Path.GetInvalidFileNameChars()));

                var query = new GetVodQueryParams { Sig = vodTwitchAuth.Sig, Token = vodTwitchAuth.Token };

                HttpContent httpContent = await _usherTwitch.GetVodAsync(video.Id, query);

                byte[] bytes = await httpContent.ReadAsByteArrayAsync(token);

                await File.WriteAllBytesAsync(validFileName, bytes, token);

                _logger.LogInformation("Видео '{0}' загружено", validFileName);

            });

            await Task.WhenAll(tasks);
        }

    }
}
