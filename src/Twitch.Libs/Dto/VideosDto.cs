using System.Collections.Generic;

namespace Twitch.Libs.Dto
{
    public class VideosDto
    {
        public int Total { get; set; }

        public List<VideoDto> VideoList { get; set; } = new List<VideoDto>();
    }
}
