using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models;

public class AppOAuthToken
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    [JsonProperty("expires_in")]
    public long ExpiresIn { get; set; }
}
