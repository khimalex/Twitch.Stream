using Newtonsoft.Json;

namespace Twitch.Stream.Services.ApiTwitchTv.KrakenModels
{
   public class Data
   {
      [JsonProperty("streamPlaybackAccessToken")]
      public PlaybackAccessToken StreamPlaybackAccessToken { get; set; }
      [JsonProperty("videoPlaybackAccessToken")]
      public PlaybackAccessToken VideoPlaybackAccessToken { get; set; }
   }
}
