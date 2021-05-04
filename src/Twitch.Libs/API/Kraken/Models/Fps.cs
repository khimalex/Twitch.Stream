using System;

namespace Twitch.Libs.API.Kraken.Models
{
    public class Fps
    {
#pragma warning disable IDE1006 // Naming Styles
        public Int32 audio_only { get; set; }
        public Double chunked { get; set; }
        public Double high { get; set; }
        public Double low { get; set; }
        public Double medium { get; set; }
        public Double mobile { get; set; }
#pragma warning restore IDE1006 // Naming Styles

    }
}
