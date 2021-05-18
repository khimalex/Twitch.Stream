using Xunit;
using System;
using Microsoft.Extensions.DependencyInjection;
using Twitch.Libs.API;
using Twitch.Libs.API.Helix;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace Twitch.Libs.Tests
{
    public class HelixApiTwitchTv_Tests
    {
        private readonly IServiceProvider _sp;

        public HelixApiTwitchTv_Tests()
        {
            IHostBuilder host = Host.CreateDefaultBuilder().UseConsoleLifetime()
            .ConfigureServices((c, services) =>
            {
                services.Configure<Appsettings>(c.Configuration);
                services.ConfigureTwitchLibs(c.Configuration);
                services.AddHttpClient<IApiTwitchTv, HelixApiTwitchTv>();

            });
            _sp = host.Build().Services;

        }
        [Theory]
        [InlineData("visualstudio")]
        public void GetChannelTwitchAuthAsync_Test(String channelName)
        {
            //Given
            IApiTwitchTv api = _sp.GetRequiredService<IApiTwitchTv>();

            //When
            Dto.UsersDto userDto = api.GetChannelInfoAsync(channelName).GetAwaiter().GetResult();

            //Then
            Assert.NotNull(userDto);
            Assert.True(userDto.Users.Any());
        }

        [Theory]
        [InlineData("visualstudio")]
        public void GetChannelVideosAsync_Test(String channelName)
        {
            IApiTwitchTv api = _sp.GetRequiredService<IApiTwitchTv>();

            Dto.VideosDto res = api.GetChannelVideosAsync(channelName).GetAwaiter().GetResult();

            Assert.NotNull(res);
            Assert.True(res.Videos.Any());

        }
        [Theory]
        [InlineData("v1017740403")]
        public void GetVideoInfoAsync_Test(String videoId)
        {
            IApiTwitchTv api = _sp.GetRequiredService<IApiTwitchTv>();

            Dto.VideosDto res = api.GetVideoInfoAsync(videoId).GetAwaiter().GetResult();

            Assert.NotNull(res);
            Assert.True(res.Videos.Any());

        }
    }
}
