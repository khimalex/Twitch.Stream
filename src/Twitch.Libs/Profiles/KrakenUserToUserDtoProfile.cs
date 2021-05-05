using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class KrakenUserToUserDtoProfile : Profile
    {
        public KrakenUserToUserDtoProfile()
        {
            CreateMap<User, UserDto>()
               .ForMember(dest => dest.Id, mo => mo.MapFrom((src, dest) => src._id));
        }
    }
}
