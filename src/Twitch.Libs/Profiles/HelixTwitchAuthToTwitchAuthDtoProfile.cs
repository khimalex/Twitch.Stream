using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class HelixTwitchAuthToTwitchAuthDtoProfile : Profile
    {
        public HelixTwitchAuthToTwitchAuthDtoProfile()
        {
            CreateMap<TwitchAuth, TwitchAuthDto>()
               .ForMember(dest => dest.Sig, mo => mo.MapFrom((src, dest) => src.Data.StreamPlaybackAccessToken.Signature))
               .ForMember(dest => dest.Token, mo => mo.MapFrom((src, dest) => src.Data.StreamPlaybackAccessToken.Value));
        }
    }
}
