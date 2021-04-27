using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Stream.Dto;

namespace Twitch.Libs.Profiles
{
   internal class KrakenResolutionsToResolutionsDtoProfile : Profile
   {
      public KrakenResolutionsToResolutionsDtoProfile()
      {
         CreateMap<Resolutions, ResolutionsDto>();
      }
   }
}
