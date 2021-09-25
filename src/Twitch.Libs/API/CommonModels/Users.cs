using System.Collections.Generic;
using Newtonsoft.Json;

namespace Twitch.Libs.API.CommonModels
{
    public class Users
    {
        [JsonProperty("data")]
        public List<User> Data { get; set; }

        [JsonProperty("users")]
        public List<User> UserList { get; set; }
    }

}
