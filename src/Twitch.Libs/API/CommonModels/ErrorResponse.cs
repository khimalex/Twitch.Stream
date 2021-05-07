using System;
using Newtonsoft.Json;

namespace Twitch.Libs.API.CommonModels
{
    internal class ErrorResponse
    {
        [JsonProperty("error")]
        public String Error { get; set; }

        [JsonProperty("status")]
        public Int32 Status { get; set; }

        [JsonProperty("message")]
        public String Message { get; set; }
    }
}
