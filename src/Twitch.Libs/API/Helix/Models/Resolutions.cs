using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix.Models
{
    public class Resolutions
    {
        [JsonProperty("chunked")]
        public string Chunked { get; set; }

        [JsonProperty("high")]
        public string High { get; set; }

        [JsonProperty("low")]
        public string Low { get; set; }

        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

    }
}
