using Newtonsoft.Json;

namespace Twitch.Stream.Services.ApiTwitchTv.KrakenModels
{

   public class TwitchAuth
   {
      [JsonProperty("data")]
      public Data Data { get; set; }

   }


}
