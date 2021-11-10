using Newtonsoft.Json;

namespace Twitch.Libs.API.CommonModels;

internal class ErrorResponse
{
    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("status")]
    public int Status { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}
