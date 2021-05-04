using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Stream.Dto;

namespace Twitch.Libs.Profiles
{
    internal class KrakenThumbnailToThumbnailDtoProfile : Profile
    {
        public KrakenThumbnailToThumbnailDtoProfile()
        {
            CreateMap<Thumbnail, ThumbnailDto>();
        }
    }
}
