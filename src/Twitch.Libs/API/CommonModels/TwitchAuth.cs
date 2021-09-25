using Newtonsoft.Json;

namespace Twitch.Libs.API.CommonModels
{
    public class TwitchAuth
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("sig")]
        public string Sig { get; set; }

        [JsonProperty("mobile_restricted")]
        public bool MobileRestricted { get; set; }

    }
}
