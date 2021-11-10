using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models;

public class Channel
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("display_name")]
    public string DisplayName { get; set; }

}
