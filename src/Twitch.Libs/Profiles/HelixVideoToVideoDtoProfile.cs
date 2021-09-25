using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class HelixVideoToVideoDtoProfile : Profile
    {
        public HelixVideoToVideoDtoProfile()
        {
            CreateMap<Video, VideoDto>();
            //.ForMember(dest => dest.UserLogin, mo => mo.MapFrom((src, dest) => src.UserLogin))
            //.ForMember(dest => dest.UserId, mo => mo.MapFrom((src, dest) => src.UserId))
            //.ForMember(dest => dest.CreatedAt, mo => mo.MapFrom((src, dest) => src.CreatedAt))
            //;
        }
    }
}
