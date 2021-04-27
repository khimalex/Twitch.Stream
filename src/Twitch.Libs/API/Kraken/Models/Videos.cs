using System;
using System.Collections.Generic;
namespace Twitch.Libs.API.Kraken.Models
{
   public class Videos
   {
#pragma warning disable IDE1006 // Naming Styles
      public Int32 _total { get; set; }
      public List<Video> videos { get; set; }
#pragma warning restore IDE1006 // Naming Styles

   }
}
