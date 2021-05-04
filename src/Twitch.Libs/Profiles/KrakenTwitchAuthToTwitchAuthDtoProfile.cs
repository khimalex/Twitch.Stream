using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Stream.Dto;

namespace Twitch.Libs.Profiles
{
    internal class KrakenTwitchAuthToTwitchAuthDtoProfile : Profile
    {
        public KrakenTwitchAuthToTwitchAuthDtoProfile()
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
