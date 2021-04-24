using AutoMapper;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Profiles
{
   internal class KrakenUserToUserDtoProfile : Profile
   {
      public KrakenUserToUserDtoProfile()
      {
         CreateMap<User, UserDto>()
            .ForMember(dest => dest.Id, mo => mo.MapFrom((src, dest) => src._id));
      }
   }
}
