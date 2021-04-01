using System;

namespace Twitch.Stream.Services.ApiTwitchTv.KrakenModels
{
   public class User
   {
#pragma warning disable IDE1006 // Naming Styles
      public String _id { get; set; }
      public String name { get; set; }

      public String display_name { get; set; }
      public String type { get; set; }
      public String bio { get; set; }
      public String logo { get; set; }
#pragma warning restore IDE1006 // Naming Styles

   }
}