using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class HelixUsersToUsersDtoProfile : Profile
    {
        public HelixUsersToUsersDtoProfile()
        {
            CreateMap<Users, UsersDto>()
               .ForMember(dest => dest.Total, mo => mo.MapFrom((src, dest) => src.data.Count))
               .ForMember(dest => dest.Users, mo => mo.MapFrom((src, dest) => src.data));
        }
    }
}
