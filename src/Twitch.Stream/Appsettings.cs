using Twitch.Libs.API;

namespace Twitch.Stream
{
    public class Appsettings
    {
        public ApiSettings TwitchApis { get; set; }
        public string[] Streams { get; set; }
    }

}
