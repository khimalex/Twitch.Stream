using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   internal class KrakenChannelToChannelDtoProfile : Profile
   {
      public KrakenChannelToChannelDtoProfile()
      {
         CreateMap<Channel, ChannelDto>()
            .ForMember(dest => dest.Id, mo => mo.MapFrom((src, dest) => src._id));
      }
   }

}
