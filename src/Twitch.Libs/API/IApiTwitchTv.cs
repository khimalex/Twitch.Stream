using System.Threading.Tasks;
using Twitch.Libs.Dto;

namespace Twitch.Libs.API
{
    public interface IApiTwitchTv
    {
        Task<TwitchAuthDto> GetChannelTwitchAuthAsync(string channelName);
        Task<UsersDto> GetChannelInfoAsync(string channelName);
        Task<VideosDto> GetChannelVideosAsync(string channelName, int first = 10, string type = "archive");
        Task<VideosDto> GetVideoInfoAsync(string videoId);
        Task<TwitchAuthDto> GetVodTwitchAuthAsync(string videoId);
        //Task<TwitchUserAuthencicationInfoDto> GetUserAuthencicationInfoDto(String clientId, String clientSecret);

    }
}
