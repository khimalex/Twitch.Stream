using AutoMapper;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Libs.Dto;

namespace Twitch.Libs.Profiles
{
    internal class KrakenThumbnailsToThumbnailsDtoProfile : Profile
    {
        public KrakenThumbnailsToThumbnailsDtoProfile()
        {
            CreateMap<Thumbnails, ThumbnailsDto>();
        }
    }
}
