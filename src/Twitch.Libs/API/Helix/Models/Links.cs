using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models;

public class Links
{
    [JsonProperty("self")]
    public string Self { get; set; }

    [JsonProperty("channel")]
    public string Channel { get; set; }
}
