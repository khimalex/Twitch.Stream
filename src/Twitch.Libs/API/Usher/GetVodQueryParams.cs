using Refit;

namespace Twitch.Libs.API.Usher
{
    public class GetVodQueryParams
    {
        [AliasAs("nauth")]
        public string Token { get; set; }

        [AliasAs("nauthsig")]
        public string Sig { get; set; }
    }
}
