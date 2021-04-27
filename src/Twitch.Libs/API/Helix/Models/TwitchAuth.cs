using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models
{
   public class TwitchAuth
   {
      [JsonProperty("data")]
      public Data Data { get; set; }

   }
}
