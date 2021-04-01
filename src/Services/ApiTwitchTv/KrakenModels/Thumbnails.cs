using System.Collections.Generic;

namespace Twitch.Stream.Services.ApiTwitchTv.KrakenModels
{
#pragma warning disable IDE1006 // Naming Styles
   public class Thumbnails
   {

      public List<Thumbnail> large { get; set; }
      public List<Thumbnail> medium { get; set; }
      public List<Thumbnail> small { get; set; }
      public List<Thumbnail> template { get; set; }

   }
#pragma warning restore IDE1006 // Naming Styles
}