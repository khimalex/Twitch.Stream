using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.HelixModels;

namespace Twitch.Stream.Profiles
{
   class HelixTwitchAuthToTwitchAuthDtoProfile : Profile
   {
      public HelixTwitchAuthToTwitchAuthDtoProfile()
      {
         CreateMap<TwitchAuth, TwitchAuthDto>()
            .ForMember(dest=> dest.Sig, mo => mo.MapFrom((src, dest) => src.Data.StreamPlaybackAccessToken.Signature))
            .ForMember(dest=> dest.Token, mo => mo.MapFrom((src, dest) => src.Data.StreamPlaybackAccessToken.Value));
      }
   }
}
