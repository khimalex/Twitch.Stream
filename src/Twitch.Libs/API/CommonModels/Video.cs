using System;
using System.Collections.Generic;

namespace Twitch.Libs.API.CommonModels
{
   public class Video
   {
#pragma warning disable IDE1006 // Naming Styles
      public String id { get; set; }
      public String user_id { get; set; }
      public String user_name { get; set; }
      public String title { get; set; }
      public String description { get; set; }
      public String created_at { get; set; }
      public String published_at { get; set; }
      public String url { get; set; }
      public String thumbnail_url { get; set; }
      public String viewable { get; set; }
      public Int64 view_count { get; set; }
      public String language { get; set; }
      public String type { get; set; }
      public String duration { get; set; }

      public Int64 broadcast_id { get; set; }
      public String status { get; set; }
      public String tag_list { get; set; }
      public String _id { get; set; }
      public String recorded_at { get; set; }
      public String game { get; set; }
      public Int32 length { get; set; }
      public String preview { get; set; }
      public Int32 views { get; set; }
      public String broadcast_type { get; set; }
      public Links _links { get; set; }
      public Channel channel { get; set; }
      public String animated_preview { get; set; }
      public List<Thumbnail> thumbnails { get; set; }
      public Fps fps { get; set; }
      public Resolutions resolutions { get; set; }
#pragma warning restore IDE1006 // Naming Styles

   }
}
