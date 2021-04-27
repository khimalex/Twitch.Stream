using System;
using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models
{
   public class PlaybackAccessToken
   {
      public String Value { get; set; }
      public String Signature { get; set; }
      [JsonProperty("__typename")]
      public String TypeName { get; set; }
   }
}
