using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class HelixUserToUserDtoProfile : Profile
    {
        public HelixUserToUserDtoProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
