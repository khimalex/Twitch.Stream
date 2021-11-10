using System.Collections.Generic;
using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models;

public class Videos
{
    [JsonProperty("data")]
    public List<Video> VideoList { get; set; }

}
