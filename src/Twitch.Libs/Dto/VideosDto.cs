using System.Collections.Generic;

namespace Twitch.Libs.Dto
{
    public class VideosDto
    {
        public System.Int32 Total { get; set; }
        public List<VideoDto> Videos { get; set; } = new List<VideoDto>();
    }
}
