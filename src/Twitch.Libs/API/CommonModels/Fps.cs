using Newtonsoft.Json;

namespace Twitch.Libs.API.CommonModels
{
    public class Fps
    {
        [JsonProperty("audio_only")]
        public int AudioOnly { get; set; }
        [JsonProperty("chunked")]
        public double Chunked { get; set; }
        [JsonProperty("high")]
        public double High { get; set; }
        [JsonProperty("low")]
        public double Low { get; set; }
        [JsonProperty("medium")]
        public double Medium { get; set; }
        [JsonProperty("mobile")]
        public double Mobile { get; set; }

    }
}
