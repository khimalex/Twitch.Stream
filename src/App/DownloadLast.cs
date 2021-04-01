﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitch.Stream.Services.ApiTwitchTv;
using Twitch.Stream.Services.UsherTwitchTv;

namespace Twitch.Stream.App
{
   class DownloadLast : IApp
   {
      private readonly ILogger _logger;
      private readonly IApiTwitchTv _apiTwitch;
      private readonly UsherTwitchTv _usherTwitch;
      private readonly Appsettings _options;

      public DownloadLast(ILogger<DownloadInfo> logger, IOptions<Appsettings> optionsAccessor, KrakenApiTwitchTv apiTwitch, UsherTwitchTv usherTwitch)
      {
         _logger = logger;
         _apiTwitch = apiTwitch;
         _usherTwitch = usherTwitch;
         _options = optionsAccessor.Value;
      }

      public async Task RunAsync(CancellationToken token = default)
      {
         String channelName = _options.Streams.First();
         var users = await _apiTwitch.GetUserInfoAsync(channelName);
         if (!users.Users.Any())
         {
            throw new Exception($"Не найден канал '{channelName}'.");
         }

         String userId = users.Users[0].Id;

         var videos = await _apiTwitch.GetUserVideosAsync(userId, 100);

         var lastVideo = videos.Videos.FirstOrDefault();
         if (lastVideo is null)
         {
            throw new Exception($"Не найдено последнего видео стримера '{channelName}'.");
         }
         var vodTwitchAuth = await _apiTwitch.GetVodTwitchAuthAsync(lastVideo.Id);

         String invalidFileName = $@"{lastVideo.Broadcast_type} {lastVideo.Channel.Name} {lastVideo.Game} {lastVideo.Created_at.ToLocalTime()}.m3u8";
         String validFileName = String.Join(" ", invalidFileName.Split(Path.GetInvalidFileNameChars()));

         Byte[] videoData = await _usherTwitch.GetVideoAsync(lastVideo.Id, vodTwitchAuth);

         await File.WriteAllBytesAsync(validFileName, videoData, token);
         _logger.LogInformation(@"Последнее видео канала '{channelName}' '{filename}' загружено", channelName, validFileName);


      }
   }
}
