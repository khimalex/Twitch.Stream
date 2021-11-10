﻿using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles;

internal class HelixUsersToUsersDtoProfile : Profile
{
    public HelixUsersToUsersDtoProfile()
    {
        _ = CreateMap<Users, UsersDto>()
           .ForMember(dest => dest.Total, mo => mo.MapFrom((src, dest) => src.Data.Count))
           .ForMember(dest => dest.UserList, mo => mo.MapFrom((src, dest) => src.Data));
    }
}
