using System.Collections.Generic;

namespace Twitch.Libs.API.Kraken.Models
{
   public class Users
   {

#pragma warning disable IDE1006 // Naming Styles
      public System.Int32 _total { get; set; }
      public List<User> users { get; set; }
#pragma warning restore IDE1006 // Naming Styles
   }

}
