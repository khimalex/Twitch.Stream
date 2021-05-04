using System;

namespace Twitch.Stream.Dto
{
    public class FpsDto
    {
        public Int32 Audio_only { get; set; }
        public Double Chunked { get; set; }
        public Double High { get; set; }
        public Double Low { get; set; }
        public Double Medium { get; set; }
        public Double Mobile { get; set; }

    }
}
