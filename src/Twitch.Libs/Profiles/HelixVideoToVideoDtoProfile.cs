﻿using AutoMapper;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles;

internal class HelixVideoToVideoDtoProfile : Profile
{
    public HelixVideoToVideoDtoProfile()
    {
        _ = CreateMap<Video, VideoDto>();
    }
}
