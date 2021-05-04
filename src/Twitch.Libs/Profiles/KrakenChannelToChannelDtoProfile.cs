using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Stream.Dto;

namespace Twitch.Libs.Profiles
{
    internal class KrakenChannelToChannelDtoProfile : Profile
    {
        public KrakenChannelToChannelDtoProfile()
        {
            CreateMap<Channel, ChannelDto>()
               .ForMember(dest => dest.Id, mo => mo.MapFrom((src, dest) => src._id));
        }
    }

}
