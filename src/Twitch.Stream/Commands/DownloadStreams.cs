using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitch.Libs.API;
using Twitch.Libs.API.Usher;
using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands
{
    internal class DownloadStreams : IApp
    {
        private readonly Appsettings _optionsAccessor;
        private readonly ILogger<DownloadStreams> _logger;
        private readonly IApiTwitchTv _apiTwitch;
        private readonly IUsherTwitchTv _usherTwitch;

        public DownloadStreams(ILogger<DownloadStreams> logger, IOptions<Appsettings> optionsAccessor, IApiTwitchTv apiTwitch, IUsherTwitchTv usherTwitch)
        {
            _optionsAccessor = optionsAccessor.Value;
            _logger = logger;
            _apiTwitch = apiTwitch;
            _usherTwitch = usherTwitch;
        }
        public async Task RunAsync(CancellationToken token = default)
        {
            //await Task.Yield();
            //for (int i = 0; i < 1000; i++)
            //{
            //   _logger.LogWarning("Test: {i}", i);
            //}
            System.Collections.Generic.IEnumerable<Task> tasks = _optionsAccessor.Streams.Select(async user =>
            {
                try
                {
                    string fileName = $"{user}.m3u8";
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    //var twitchauth = await _helixApiTwitchTv.GetChannelTwitchAuthAsync(user);
                    TwitchAuthDto twitchauth = await _apiTwitch.GetChannelTwitchAuthAsync(user);

                    var query = new GetStreamQueryParams
                    {
                        Sig = twitchauth.Sig,
                        Token = twitchauth.Token
                    };

                    HttpContent httpContent = await _usherTwitch.GetStreamAsync(user, query);
                    
                    byte[] bytes = await httpContent.ReadAsByteArrayAsync(token);
                    
                    await File.WriteAllBytesAsync(fileName, bytes);
                    
                    _logger.LogInformation("Стрим '{user}' загружен в '{file}'!", user, fileName);
                }
                catch (Exception e)
                {
                    _logger.LogError("Ошибка загрузки стрима '{user}': {message}", user, e.Message);
                }

            });

            await Task.WhenAll(tasks);

        }

    }
}
