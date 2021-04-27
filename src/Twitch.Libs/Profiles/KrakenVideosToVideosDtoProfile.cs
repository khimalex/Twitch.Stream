using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Stream.Dto;

namespace Twitch.Libs.Profiles
{
   internal class KrakenVideosToVideosDtoProfile : Profile
   {
      public KrakenVideosToVideosDtoProfile()
      {
         CreateMap<Videos, VideosDto>()
            .ForMember(dest => dest.Total, mo => mo.MapFrom((src, dest) => src._total));
      }
   }
}
