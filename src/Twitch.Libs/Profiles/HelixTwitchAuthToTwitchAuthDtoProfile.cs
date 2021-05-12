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
               .ForMember(dest => dest.Sig, mo => mo.MapFrom((src, dest) =>
               {
                   if (src.Data.StreamPlaybackAccessToken is not null)
                   {
                       return src.Data.StreamPlaybackAccessToken.Signature;
                   }
                   else
                   {
                       return src.Data.VideoPlaybackAccessToken.Signature;
                   }
               }))
               .ForMember(dest => dest.Token, mo => mo.MapFrom((src, dest) =>
               {
                   if (src.Data.StreamPlaybackAccessToken is not null)
                   {
                       return src.Data.StreamPlaybackAccessToken.Value;
                   }
                   else
                   {
                       return src.Data.VideoPlaybackAccessToken.Value;
                   }
               }));
        }
    }
}
