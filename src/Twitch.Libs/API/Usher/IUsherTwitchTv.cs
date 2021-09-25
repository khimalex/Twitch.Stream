using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace Twitch.Libs.API.Usher
{
    [Headers("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3952.0 Safari/537.36 Edg/80.0.320.5")]
    public interface IUsherTwitchTv
    {
        [Get("/api/channel/hls/{channelName}.m3u8")]
        // Для того, Чтобы получить байты, указываем HttpContent, TODO: установить декоратор, пусть всё делает под капотом
        public Task<HttpContent> GetStreamAsync(string channelName, GetStreamQueryParams query);

        [Get("/vod/{vodId}")]
        // Для того, Чтобы получить байты, указываем HttpContent, TODO: установить декоратор, пусть всё делает под капотом
        public Task<HttpContent> GetVodAsync(string vodId, GetVodQueryParams query);
    }
}