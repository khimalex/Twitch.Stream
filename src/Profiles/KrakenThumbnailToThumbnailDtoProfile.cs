using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   class KrakenThumbnailToThumbnailDtoProfile : Profile
   {
      public KrakenThumbnailToThumbnailDtoProfile()
      {
         CreateMap<Thumbnail, ThumbnailDto>();
      }
   }
}
