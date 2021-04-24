using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   internal class KrakenPreviewToVPreviewDtoProfile : Profile
   {
      public KrakenPreviewToVPreviewDtoProfile()
      {
         CreateMap<Preview, PreviewDto>();
      }
   }
}
