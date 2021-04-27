using Newtonsoft.Json;

namespace Twitch.Libs.API.Kraken.Models
{
   public class Data
   {
      [JsonProperty("streamPlaybackAccessToken")]
      public PlaybackAccessToken StreamPlaybackAccessToken { get; set; }
      [JsonProperty("videoPlaybackAccessToken")]
      public PlaybackAccessToken VideoPlaybackAccessToken { get; set; }
   }
}
