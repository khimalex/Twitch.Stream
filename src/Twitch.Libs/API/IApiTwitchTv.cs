using System.Threading.Tasks;
using Twitch.Libs.Dto;

namespace Twitch.Libs.API
{
    public interface IApiTwitchTv
    {
        Task<TwitchAuthDto> GetChannelTwitchAuthAsync(System.String channelName);
        Task<UsersDto> GetChannelInfoAsync(System.String channelName);
        Task<VideosDto> GetChannelVideosAsync(System.String channelName, System.Int32 first = 10, System.String type = "archive");
        Task<VideosDto> GetVideoInfoAsync(System.String videoId);
        Task<TwitchAuthDto> GetVodTwitchAuthAsync(System.String videoId);
        //Task<TwitchUserAuthencicationInfoDto> GetUserAuthencicationInfoDto(String clientId, String clientSecret);

    }
}
