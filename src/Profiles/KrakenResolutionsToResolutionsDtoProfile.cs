using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   class KrakenResolutionsToResolutionsDtoProfile : Profile
   {
      public KrakenResolutionsToResolutionsDtoProfile()
      {
         CreateMap<Resolutions, ResolutionsDto>();
      }
   }
}
