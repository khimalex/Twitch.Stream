using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   internal class KrakenThumbnailsToThumbnailsDtoProfile : Profile
   {
      public KrakenThumbnailsToThumbnailsDtoProfile()
      {
         CreateMap<Thumbnails, ThumbnailsDto>();
      }
   }
}
