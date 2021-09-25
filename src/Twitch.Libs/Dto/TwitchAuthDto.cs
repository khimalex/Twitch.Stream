namespace Twitch.Libs.Dto
{
    public class TwitchAuthDto
    {
        public string Token { get; set; }

        public string Sig { get; set; }

        public bool MobileRestricted { get; set; }

    }
}
