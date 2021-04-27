using Newtonsoft.Json;

namespace Twitch.Libs.API.Kraken.Models
{

   public class TwitchAuth
   {
      [JsonProperty("data")]
      public Data Data { get; set; }

   }


}
