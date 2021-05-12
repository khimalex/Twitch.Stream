using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class HelixVideosToVideosDtoProfile : Profile
    {
        public HelixVideosToVideosDtoProfile()
        {
            CreateMap<Videos, VideosDto>()
               .ForMember(dest => dest.Total, mo => mo.MapFrom((src, dest) => src.Data.Count))
               .ForMember(dest => dest.Videos, mo => mo.MapFrom((src, dest) => src.Data))
               ;
        }
    }
}
