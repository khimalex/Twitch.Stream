using Newtonsoft.Json;

namespace Twitch.Stream.Services.ApiTwitchTv.HelixModels
{
   public class TwitchAuth
   {
      [JsonProperty("data")]
      public Data Data { get; set; }

   }
}
