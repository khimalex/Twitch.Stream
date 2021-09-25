using Newtonsoft.Json;

namespace Twitch.Libs.API.CommonModels
{
    public class Links
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }
    }
}
