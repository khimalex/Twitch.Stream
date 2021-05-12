using Xunit;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Twitch.Libs.API;
using Twitch.Libs.API.Helix;
using System.Linq;

namespace Twitch.Libs.Tests
{
    public class HelixApiTwitchTv_Tests
    {
        private readonly ServiceProvider _sp;

        public HelixApiTwitchTv_Tests()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = configurationBuilder.Build();
            var services = new ServiceCollection();
            services.Configure<Appsettings>(configuration);
            services.ConfigureTwitchLibs(configuration);
            services.AddHttpClient<IApiTwitchTv, HelixApiTwitchTv>();
            _sp = services.BuildServiceProvider();

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
        [InlineData("juice")]
        public void GetChannelVideosAsync_Test(String channelName)
        {
            IApiTwitchTv api = _sp.GetRequiredService<IApiTwitchTv>();

            Dto.VideosDto res = api.GetChannelVideosAsync(channelName).GetAwaiter().GetResult();

            Assert.NotNull(res);
            Assert.True(res.Videos.Any());

        }
        [Theory]
        [InlineData("v1017740403")]
        public void GetVideoInfoAsync_Test(String channelName)
        {
            IApiTwitchTv api = _sp.GetRequiredService<IApiTwitchTv>();

            Dto.VideosDto res = api.GetVideoInfoAsync(channelName).GetAwaiter().GetResult();

            Assert.NotNull(res);
            Assert.True(res.Videos.Any());

        }
    }
}
