using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Stream.Dto;

namespace Twitch.Libs.Profiles
{
   internal class KrakenUsersToUsersDtoProfile : Profile
   {
      public KrakenUsersToUsersDtoProfile()
      {
         CreateMap<Users, UsersDto>()
            .ForMember(dest => dest.Total, mo => mo.MapFrom((src, dest) => src._total));
      }
   }
}
