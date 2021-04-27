using System;

namespace Twitch.Libs.API.CommonModels
{
   public class TwitchAuth
   {
#pragma warning disable IDE1006 // Naming Styles
      public String token { get; set; }
      public String sig { get; set; }
      public Boolean mobile_restricted { get; set; }
#pragma warning restore IDE1006 // Naming Styles

   }
}
