using System;
using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models
{
    public class AppOAuthToken
    {
        [JsonProperty("access_token")]
        public String AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public Int64 ExpiresIn { get; set; }
    }
}
