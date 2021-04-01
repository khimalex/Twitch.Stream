using Newtonsoft.Json;

namespace Twitch.Stream.Services.ApiTwitchTv.HelixModels
{
   public class Data
   {
      [JsonProperty("streamPlaybackAccessToken")]
      public PlaybackAccessToken StreamPlaybackAccessToken { get; set; }
   }
}
