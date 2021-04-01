using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   class KrakenVideosToVideosDtoProfile : Profile
   {
      public KrakenVideosToVideosDtoProfile()
      {
         CreateMap<Videos, VideosDto>()
            .ForMember(dest => dest.Total, mo => mo.MapFrom((src, dest) => src._total));
      }
   }
}
