using System.Collections.Generic;

namespace Twitch.Libs.Dto
{
    public class UsersDto
    {
        public System.Int32 Total { get; set; }
        public List<UserDto> Users { get; set; }
    }
}
