using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models
{
    public class PlaybackAccessToken
    {
        public string Value { get; set; }
        public string Signature { get; set; }
        [JsonProperty("__typename")]
        public string TypeName { get; set; }
    }
}
