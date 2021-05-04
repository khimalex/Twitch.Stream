using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Stream.Dto;

namespace Twitch.Libs.Profiles
{
    internal class KrakenPreviewToVPreviewDtoProfile : Profile
    {
        public KrakenPreviewToVPreviewDtoProfile()
        {
            CreateMap<Preview, PreviewDto>();
        }
    }
}
