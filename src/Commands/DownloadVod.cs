using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv;
using Twitch.Stream.Services.UsherTwitchTv;

namespace Twitch.Stream.Commands
{
   internal class DownloadVod : IApp
   {
      private readonly Appsettings _options;
      private readonly ILogger _logger;
      private readonly IApiTwitchTv _apiTwitch;
      private readonly UsherTwitchTv _usherTwitch;

      public DownloadVod(ILogger<DownloadVod> logger, IOptions<Appsettings> optionsAccessor, KrakenApiTwitchTv apiTwitch, UsherTwitchTv usherTwitch)
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
            if (!videos.Videos.Any())
            {
               throw new Exception($"Не найдено видео '{videoId}'.");

            }
            VideoDto video = videos.Videos.First();

            TwitchAuthDto vodTwitchAuth = await _apiTwitch.GetVodTwitchAuthAsync(videoId);

            String invalidFileName = $@"{video.Broadcast_type} {video.Channel.Name} {video.Game} {video.Created_at.ToLocalTime()}.m3u8";
            String validFileName = String.Join(" ", invalidFileName.Split(Path.GetInvalidFileNameChars()));

            Byte[] videoData = await _usherTwitch.GetVideoAsync(video.Id, vodTwitchAuth);

            await File.WriteAllBytesAsync(validFileName, videoData);
            _logger.LogInformation("Видео '{0}' загружено", validFileName);

         });

         await Task.WhenAll(tasks);
      }

   }
}
