using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles;

internal class HelixUserToUserDtoProfile : Profile
{
    public HelixUserToUserDtoProfile()
    {
        _ = CreateMap<User, UserDto>();
    }
}
