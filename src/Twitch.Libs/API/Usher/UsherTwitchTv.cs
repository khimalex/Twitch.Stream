using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Twitch.Libs.Dto;

namespace Twitch.Libs.API.Usher
{
    public class UsherTwitchTv
    {
        private readonly UsherSettings _options;
        private readonly HttpClient _client;

        public UsherTwitchTv(HttpClient c, IOptions<UsherSettings> options)
        {
            _options = options.Value;
            _client = c;
        }


        public async Task<Byte[]> GetStreamAsync(String channelName, TwitchAuthDto channelTwitchAuth)
        {
            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Get,
                requestUri: $@"http://usher.twitch.tv/api/channel/hls/{channelName}.m3u8?player=twitchweb&&token={channelTwitchAuth.Token}&sig={channelTwitchAuth.Sig}&allow_audio_only=true&allow_source=true&type=any&p=2301211'",
                headers: new Dictionary<String, String>()
                {
                    {"Client-ID", _options.ClientIDWeb},
                    {"User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3952.0 Safari/537.36 Edg/80.0.320.5"}
                }
            );
            using HttpResponseMessage response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($@"Стрим '{channelName}' не загружен. Статус ошибки: {(Int32)response.StatusCode,3}.");
            }
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<Byte[]> GetVideoAsync(String vodId, TwitchAuthDto vodTwitchAuth)
        {
            vodId = new String(vodId.Where(c => Char.IsDigit(c)).ToArray());

            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Get,
                requestUri: $@"http://usher.twitch.tv/vod/{vodId}?nauthsig={vodTwitchAuth.Sig}&nauth={vodTwitchAuth.Token}",
                headers: new Dictionary<String, String>()
                {
                    {"Client-ID", _options.ClientIDWeb},
                    {"User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3952.0 Safari/537.36 Edg/80.0.320.5"}
                }
            );
            using HttpResponseMessage response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                String str = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($@"Видео '{vodId}' не загружено. Статус ошибки: {(Int32)response.StatusCode,15}.");
            }
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
