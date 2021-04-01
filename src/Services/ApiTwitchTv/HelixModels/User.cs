using System;

namespace Twitch.Stream.Services.ApiTwitchTv.HelixModels
{
   public class User
   {
#pragma warning disable IDE1006 // Naming Styles
      public String id { get; set; }
      public String login { get; set; }

      public String display_name { get; set; }
      public String type { get; set; }
      public String broadcaster_type { get; set; }
      public String description { get; set; }
      public String profile_image_url { get; set; }
      public String offline_image_url { get; set; }
      public Int64 view_count { get; set; }
      public String email { get; set; }
#pragma warning restore IDE1006 // Naming Styles

   }
}