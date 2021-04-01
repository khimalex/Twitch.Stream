using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   class KrakenFpsToFpsDtoProfile : Profile
   {
      public KrakenFpsToFpsDtoProfile()
      {
         CreateMap<Fps, FpsDto>();
      }
   }

}
