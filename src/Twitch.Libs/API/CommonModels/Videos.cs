using System.Collections.Generic;
using Newtonsoft.Json;

namespace Twitch.Libs.API.CommonModels;

public class Videos
{
    [JsonProperty("data")]
    public List<Video> Data { get; set; }

}
