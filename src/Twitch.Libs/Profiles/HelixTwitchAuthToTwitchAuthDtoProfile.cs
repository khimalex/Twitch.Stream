using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles;

internal class HelixTwitchAuthToTwitchAuthDtoProfile : Profile
{
    public HelixTwitchAuthToTwitchAuthDtoProfile()
    {
        _ = CreateMap<TwitchAuth, TwitchAuthDto>()
           .ForMember(dest => dest.Sig, mo => mo.MapFrom((src, dest) =>
           {
               return src.Data.StreamPlaybackAccessToken switch
               {
                   not null => src.Data.StreamPlaybackAccessToken.Signature,
                   _ => src.Data.VideoPlaybackAccessToken.Signature
               };
           }))
           .ForMember(dest => dest.Token, mo => mo.MapFrom((src, dest) =>
           {
               return src.Data.StreamPlaybackAccessToken switch
               {
                   not null => src.Data.StreamPlaybackAccessToken.Value,
                   _ => src.Data.VideoPlaybackAccessToken.Value
               };
           }));
    }
}
