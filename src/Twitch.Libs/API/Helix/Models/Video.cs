using System;

namespace Twitch.Libs.API.Helix.Models
{
    public class Video
    {
#pragma warning disable IDE1006 // Naming Styles
        public String id { get; set; }
        public String stream_id { get; set; }
        public String user_id { get; set; }
        public String user_login { get; set; }
        public String user_name { get; set; }
        public String title { get; set; }
        public String description { get; set; }
        public String created_at { get; set; }
        public String published_at { get; set; }
        public String language { get; set; }
        public String type { get; set; }
        public String duration { get; set; }

#pragma warning restore IDE1006 // Naming Styles

    }
}
