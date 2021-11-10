using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models;

public class Thumbnail
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

}
