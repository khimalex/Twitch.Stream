using Newtonsoft.Json;

namespace Twitch.Libs.API.CommonModels;

public class Thumbnail
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}
