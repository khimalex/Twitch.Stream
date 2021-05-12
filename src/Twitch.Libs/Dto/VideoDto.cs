using System;

namespace Twitch.Libs.Dto
{
    public class VideoDto
    {
        public String Id { get; set; }
        public String Game { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public String Language { get; set; }
        public String UserId { get; set; }
        public String Login { get; set; }
        // public ChannelDto Channel { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
