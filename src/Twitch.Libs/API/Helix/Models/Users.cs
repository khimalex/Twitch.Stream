using System.Collections.Generic;

namespace Twitch.Libs.API.Helix.Models
{
   public class Users
   {
#pragma warning disable IDE1006 // Naming Styles
      public List<User> data { get; set; }
      public List<User> users { get; set; }
#pragma warning restore IDE1006 // Naming Styles
   }

}
