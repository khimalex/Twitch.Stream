using System.Threading.Tasks;
using Twitch.Stream.Dto;

namespace Twitch.Libs.API
{
    public interface IApiTwitchTv
    {
        Task<TwitchAuthDto> GetChannelTwitchAuthAsync(System.String channelName);
        Task<UsersDto> GetUserInfoAsync(System.String channelName);
        Task<VideosDto> GetUserVideosAsync(System.String userId, System.Int32 first = 10, System.String type = "archive");
        Task<VideosDto> GetVideoInfoAsync(System.String videoId);
        Task<TwitchAuthDto> GetVodTwitchAuthAsync(System.String videoId);

    }
}
