using Refit;

namespace Twitch.Libs.API.Usher;

public class GetStreamQueryParams
{
    [AliasAs("player")]
    public string Player { get; set; } = "twitchweb";

    [AliasAs("token")]
    public string Token { get; set; }

    [AliasAs("sig")]
    public string Sig { get; set; }

    [AliasAs("allow_audio_only")]
    public bool AllowAudioOnly { get; set; } = true;

    [AliasAs("allow_source")]
    public bool AllowSource { get; set; } = true;

    [AliasAs("type")]
    public string Type { get; set; } = "any";

    [AliasAs("p")]
    public string P { get; set; } = "2301211'";
}
