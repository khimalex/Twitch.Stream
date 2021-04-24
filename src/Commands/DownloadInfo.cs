using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitch.Stream.Services.ApiTwitchTv;

namespace Twitch.Stream.Commands
{
   internal class DownloadInfo : IApp
   {
      private readonly ILogger _logger;
      private readonly IApiTwitchTv _apiTwitch;
      private readonly Appsettings _options;

      public DownloadInfo(ILogger<DownloadInfo> logger, IOptions<Appsettings> optionsAccessor, KrakenApiTwitchTv apiTwitch)
      {
         _logger = logger;
         _apiTwitch = apiTwitch;
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
         videos.Videos.Reverse();
         foreach (var item in videos.Videos)
         {
            Console.WriteLine($@"{"channel",15}: {item.Channel.Name}");
            Console.WriteLine($@"{"id",15}: {item.Id}");
            Console.WriteLine($@"{"broadcast_type",15}: {item.Broadcast_type}");
            Console.WriteLine($@"{"title",15}: {item.Title}");
            Console.WriteLine($@"{"game",15}: {item.Game}");
            Console.WriteLine($@"{"DateTime",15}: {item.Created_at.ToLocalTime()}");
            Console.WriteLine(new String('-', Console.WindowWidth));
         }
      }
   }
}
