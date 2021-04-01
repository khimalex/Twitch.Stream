using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   class KrakenTwitchAuthToTwitchAuthDtoProfile : Profile
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
