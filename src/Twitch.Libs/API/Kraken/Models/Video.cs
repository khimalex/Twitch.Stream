using System;

namespace Twitch.Libs.API.Kraken.Models
{
   public class Video
   {
#pragma warning disable IDE1006 // Naming Styles
      public String _id { get; set; }
      public String broadcast_id { get; set; }
      public String broadcast_type { get; set; }
      public Channel channel { get; set; }
      public DateTime created_at { get; set; }
      public String description { get; set; }
      public String description_html { get; set; }
      public Fps fps { get; set; }
      public String game { get; set; }
      public String language { get; set; }
      public String length { get; set; }
      public Preview preview { get; set; }
      public String published_at { get; set; }
      public Resolutions resolutions { get; set; }
      public String status { get; set; }
      public String tag_list { get; set; }
      public Thumbnails thumbnails { get; set; }
      public String title { get; set; }
      public String url { get; set; }
      public String viewable { get; set; }
      public String viewable_at { get; set; }
      public String views { get; set; }
#pragma warning restore IDE1006 // Naming Styles

   }
}
