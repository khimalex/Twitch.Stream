using System.Collections.Generic;

namespace Twitch.Libs.Dto;

public class UsersDto
{
    public int Total { get; set; }

    public List<UserDto> UserList { get; set; }
}
