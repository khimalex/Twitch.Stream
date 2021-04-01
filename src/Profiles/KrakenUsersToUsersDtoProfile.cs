using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   class KrakenUsersToUsersDtoProfile : Profile
   {
      public KrakenUsersToUsersDtoProfile()
      {
         CreateMap<Users, UsersDto>()
            .ForMember(dest => dest.Total, mo => mo.MapFrom((src, dest) => src._total));
      }
   }
}
