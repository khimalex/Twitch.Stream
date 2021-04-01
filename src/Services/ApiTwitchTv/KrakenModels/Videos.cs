using System.Collections.Generic;

namespace Twitch.Stream.Services.ApiTwitchTv.KrakenModels
{
   public class Videos
   {
#pragma warning disable IDE1006 // Naming Styles
      public System.Int32 _total { get; set; }
      public List<Video> videos { get; set; }
#pragma warning restore IDE1006 // Naming Styles

   }
}
