using System;

namespace Twitch.Stream.Dto
{
    public class TwitchAuthDto
    {
        public String Token { get; set; }
        public String Sig { get; set; }
        public Boolean Mobile_restricted { get; set; }

    }
}
