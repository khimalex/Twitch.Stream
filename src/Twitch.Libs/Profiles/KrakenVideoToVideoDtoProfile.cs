using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class KrakenVideoToVideoDtoProfile : Profile
    {
        public KrakenVideoToVideoDtoProfile()
        {
            CreateMap<Video, VideoDto>()
               .ForMember(dest => dest.Id, mo => mo.MapFrom((src, dest) => src._id));
        }
    }


}
