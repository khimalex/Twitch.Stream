using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class KrakenFpsToFpsDtoProfile : Profile
    {
        public KrakenFpsToFpsDtoProfile()
        {
            CreateMap<Fps, FpsDto>();
        }
    }

}
