using System;

namespace Twitch.Libs.Dto
{
    public class TwitchAuthDto
    {
        public String Token { get; set; }
        public String Sig { get; set; }
        public Boolean Mobile_restricted { get; set; }

    }
}
