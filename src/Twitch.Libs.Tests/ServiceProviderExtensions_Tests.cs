using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Converters;
using Twitch.Libs.API;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;
using AutoMapper;
using System.Collections.Generic;
using Twitch.Libs.Profiles;

namespace Twitch.Libs.Tests
{
    public class ServiceProviderExtensions_Tests
    {
        private readonly ITestOutputHelper _output;

        public ServiceProviderExtensions_Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void JsonSerializerSettings_Prevent_Reregistration_Test()
        {
            //Given
            var jsonSerializerSettings = new JsonSerializerSettings();
            var services = new ServiceCollection();
            Int32 convertersCount = jsonSerializerSettings.Converters.Count;
            NullValueHandling nullValueHandling = jsonSerializerSettings.NullValueHandling;

            //When
            services.ConfigureJsonSerializerOptions(jsonSerializerSettings);
            ServiceProvider sp = services.BuildServiceProvider();
            JsonSerializerSettings newJsonSerializerSettings = sp.GetRequiredService<JsonSerializerSettings>();
            //Then
            Assert.Equal(jsonSerializerSettings, newJsonSerializerSettings);
            Assert.Equal(convertersCount, newJsonSerializerSettings.Converters.Count);
            Assert.Equal(nullValueHandling, newJsonSerializerSettings.NullValueHandling);
            Boolean containStringEnumConverter = newJsonSerializerSettings.Converters
                                                 .Any(c => typeof(StringEnumConverter) == c.GetType());
            Assert.False(containStringEnumConverter);
        }

        [Fact]
        public void JsonSerializerSettings_New_Settings_Have_Default_Value_Test()
        {
            //Given
            var services = new ServiceCollection();
            services.ConfigureJsonSerializerOptions();
            ServiceProvider sp = services.BuildServiceProvider();

            //When
            JsonSerializerSettings settings = sp.GetService<JsonSerializerSettings>();
            //Then
            Assert.NotNull(settings);
            Assert.Equal(NullValueHandling.Ignore, settings.NullValueHandling);
            Assert.Equal(1, settings.Converters.Count);
            Boolean containStringEnumConverter = settings.Converters.Any(c => c.GetType() == typeof(StringEnumConverter));
            Assert.True(containStringEnumConverter);
        }

        private class AppsettingsWrapper
        {
            public ApiSettings TwitchApis { get; set; }
        }
        [Fact]
        public void Configute_TwitchApi_Section_Test()
        {
            //Given
            String _twitchApisJson =
            @"
{
    ""TwitchApis"": {
                    ""ClientIDWeb"": ""kimne78kx3ncx6brgo4mv6wki5h1ko"",
                    ""ClientID"": ""cyo0r8igpepltw3op368vsy4wsub16w"",
                    ""ClientSecret"": ""You MUST obtain Your Own Here: https://dev.twitch.tv/console/apps/create""
                   }
}";

            using var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);
            sw.Write(_twitchApisJson);
            sw.Flush();
            ms.Position = 0;

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonStream(ms);
            IConfigurationRoot configuration = configurationBuilder.Build();
            var services = new ServiceCollection();
            services.Configure<AppsettingsWrapper>(configuration);
            services.ConfigureTwitchApisSection(configuration);
            ServiceProvider sp = services.BuildServiceProvider();

            //When
            AppsettingsWrapper appsettings = sp.GetService<IOptions<AppsettingsWrapper>>()?.Value;
            ApiSettings twitchApiSettings = sp.GetService<IOptions<ApiSettings>>()?.Value;

            //Then
            Assert.NotNull(appsettings);
            Assert.NotNull(twitchApiSettings);
            Assert.NotEqual(appsettings.TwitchApis, twitchApiSettings);
            Assert.Equal("cyo0r8igpepltw3op368vsy4wsub16w", twitchApiSettings.ClientID);
            Assert.Equal("kimne78kx3ncx6brgo4mv6wki5h1ko", twitchApiSettings.ClientIDWeb);
            Assert.Equal("You MUST obtain Your Own Here: https://dev.twitch.tv/console/apps/create", twitchApiSettings.ClientSecret);
        }

        [Fact]
        public void AutoMapper_Double_Registration_Test()
        {
            //Given
            var services = new ServiceCollection();
            services.AddAutoMapper(c => { });
            services.ConfigureAutoMapper();
            ServiceProvider sp = services.BuildServiceProvider();

            var registeredProfiles = new List<Type>()
            {
                typeof(HelixTwitchAuthToTwitchAuthDtoProfile),
                typeof(KrakenTwitchAuthToTwitchAuthDtoProfile),
                typeof(KrakenUsersToUsersDtoProfile),
                typeof(KrakenUserToUserDtoProfile),
                typeof(KrakenVideosToVideosDtoProfile),
                typeof(KrakenVideoToVideoDtoProfile),
                typeof(KrakenChannelToChannelDtoProfile),
                typeof(KrakenFpsToFpsDtoProfile),
                typeof(KrakenPreviewToVPreviewDtoProfile),
                typeof(KrakenResolutionsToResolutionsDtoProfile),
                typeof(KrakenThumbnailsToThumbnailsDtoProfile),
                typeof(KrakenThumbnailToThumbnailDtoProfile),
            };

            //When
            IMapper mapper = sp.GetService<IMapper>();
            var mappers = mapper.ConfigurationProvider.GetAllTypeMaps().ToList();

            //Then
            Boolean allMapperTypesNamesExists = mappers.Select(m => m.Profile.Name).All(n => registeredProfiles.Any(t => t.FullName.Equals(n)));

            Assert.Equal(12, mappers.Count);
            Assert.True(allMapperTypesNamesExists);
            // _output.WriteLine("TESTS STARTED!!!");
        }

    }

}
