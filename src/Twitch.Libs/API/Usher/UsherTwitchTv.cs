using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Twitch.Stream.Dto;

namespace Twitch.Libs.API.Usher
{
    public class UsherTwitchTv
    {
        //Affiliateed only with Usher API.
        private readonly String _streamStringFormat = @"/api/channel/hls/{0}.m3u8?player=twitchweb&&token={1}&sig={2}&allow_audio_only=true&allow_source=true&type=any&p={3}'";
        private readonly String _videoStringFormat = @"/vod/{0}?nauthsig={1}&nauth={2}";

        public UsherTwitchTv(HttpClient c, IOptions<ApiSettings> options)
        {
            //Base internal API Usher, not for external users, received by sniffing browser dev tools
            c.BaseAddress = new Uri(@"http://usher.twitch.tv");
            c.DefaultRequestHeaders.Add("Client-ID", options.Value.ClientIDWeb);
            c.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3952.0 Safari/537.36 Edg/80.0.320.5");
            Client = c;
        }
        public HttpClient Client { get; }

        public async Task<Byte[]> GetStreamAsync(String channelName, TwitchAuthDto channelTwitchAuth)
        {
            String request = String.Format(_streamStringFormat, channelName, channelTwitchAuth.Token, channelTwitchAuth.Sig, 2301211);
            HttpResponseMessage response = await Client.GetAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($@"Стрим '{channelName}' не загружен. Статус ошибки: {(Int32)response.StatusCode,3}.");
            }
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<Byte[]> GetVideoAsync(String vodId, TwitchAuthDto vodTwitchAuth)
        {
            vodId = new String(vodId.Where(c => Char.IsDigit(c)).ToArray());

            String request = String.Format(_videoStringFormat, vodId, vodTwitchAuth.Sig, vodTwitchAuth.Token);
            HttpResponseMessage response = await Client.GetAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                String str = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($@"Видео '{vodId}' не загружено. Статус ошибки: {(Int32)response.StatusCode,15}.");
            }



            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
