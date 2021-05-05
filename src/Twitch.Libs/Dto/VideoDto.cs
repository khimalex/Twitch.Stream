using System;

namespace Twitch.Libs.Dto
{
    public class VideoDto
    {
        public String Id { get; set; }
        public Int64 Broadcast_id { get; set; }
        public String Broadcast_type { get; set; }
        public ChannelDto Channel { get; set; }
        public DateTime Created_at { get; set; }
        public String Description { get; set; }
        //public String Description_html { get; set; }
        //public FpsDto Fps { get; set; }
        public String Game { get; set; }
        public String Language { get; set; }
        //public Int64 Length { get; set; }
        //public PreviewDto Preview { get; set; }
        //public DateTime Published_at { get; set; }
        //public ResolutionsDto Resolutions { get; set; }
        //public String Status { get; set; }
        //public String Tag_list { get; set; }
        //public ThumbnailsDto Thumbnails { get; set; }
        public String Title { get; set; }
        //public String Url { get; set; }
        //public String Viewable { get; set; }
        //public String Viewable_at { get; set; }
        //public Int64 Views { get; set; }

    }
}
