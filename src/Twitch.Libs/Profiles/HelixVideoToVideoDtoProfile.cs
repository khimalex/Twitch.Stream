using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class HelixVideoToVideoDtoProfile : Profile
    {
        public HelixVideoToVideoDtoProfile()
        {
            CreateMap<Video, VideoDto>()
               .ForMember(dest => dest.Login, mo => mo.MapFrom((src, dest) => src.user_login))
               .ForMember(dest => dest.UserId, mo => mo.MapFrom((src, dest) => src.user_id))
               .ForMember(dest => dest.CreatedAt, mo => mo.MapFrom((src, dest) => src.created_at))
               ;
        }
    }


}
