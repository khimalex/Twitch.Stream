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
               .ForMember(dest => dest.Total, mo => mo.MapFrom((src, dest) => src.VideoList.Count))
               .ForMember(dest => dest.VideoList, mo => mo.MapFrom((src, dest) => src.VideoList))
               ;
        }
    }
}
