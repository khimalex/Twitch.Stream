using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   class KrakenThumbnailsToThumbnailsDtoProfile : Profile
   {
      public KrakenThumbnailsToThumbnailsDtoProfile()
      {
         CreateMap<Thumbnails, ThumbnailsDto>();
      }
   }
}
