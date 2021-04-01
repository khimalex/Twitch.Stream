using System;

namespace Twitch.Stream
{
   public class Appsettings
   {
      public TwitchApi ApiVersion { get; set; } = TwitchApi.Helix;
      public String ClientIDWeb { get; set; }
      public String ClientID { get; set; }
      public String[] Streams { get; set; }
   }

   public enum TwitchApi
   {
      Helix,
      Kraken
   }

}
