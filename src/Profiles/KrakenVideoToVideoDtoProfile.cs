using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   class KrakenVideoToVideoDtoProfile : Profile
   {
      public KrakenVideoToVideoDtoProfile()
      {
         CreateMap<Video, VideoDto>()
            .ForMember(dest => dest.Id, mo => mo.MapFrom((src, dest) => src._id));
      }
   }


}
