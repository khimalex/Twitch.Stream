using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Twitch.Libs.API;
using Twitch.Libs.API.Helix;
using Twitch.Libs.API.Kraken;
using Twitch.Libs.API.Usher;
using Twitch.Libs.Exceptions.Configuration;
using Twitch.Libs.Profiles;

//Assembly of tests for this library MUST wil be named 'Twitch.Libs.Tests'.
[assembly: InternalsVisibleTo("Twitch.Libs.Tests")]
namespace Twitch.Libs
{
    /// <summary>
    /// Extensions for customized configurations of client services
    /// </summary>
    public static class ServiceProviderExtensions
    {

        public static IServiceCollection ConfigureTwitchLibs(this IServiceCollection services, IConfiguration configuration, JsonSerializerSettings jsonSerializerSettings = null)
        {
            services.ConfigureAutoMapper();
            services.ConfigureTwitchApisSection(configuration);
            services.ConfigureJsonSerializerOptions(jsonSerializerSettings);
            services.ConfigureApiContainers();

            return services;
        }

        internal static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            //Don't need checking double registration, as i think...
            //Developers checked it already, see here:
            //https://github.com/AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection/blob/be2b8cdb0b52d92fe873f0a7d1baacdd9fe0e5aa/src/AutoMapper.Extensions.Microsoft.DependencyInjection/ServiceCollectionExtensions.cs#L103
            services.AddAutoMapper(c =>
            {
                c.AddProfile<HelixTwitchAuthToTwitchAuthDtoProfile>();
                c.AddProfile<HelixUsersToUsersDtoProfile>();
                c.AddProfile<HelixUserToUserDtoProfile>();
                c.AddProfile<HelixVideoToVideoDtoProfile>();
                c.AddProfile<HelixVideosToVideosDtoProfile>();

                c.AddProfile<KrakenTwitchAuthToTwitchAuthDtoProfile>();
                c.AddProfile<KrakenUsersToUsersDtoProfile>();
                c.AddProfile<KrakenUserToUserDtoProfile>();
                c.AddProfile<KrakenVideosToVideosDtoProfile>();
                c.AddProfile<KrakenVideoToVideoDtoProfile>();
                c.AddProfile<KrakenChannelToChannelDtoProfile>();
                c.AddProfile<KrakenFpsToFpsDtoProfile>();
                c.AddProfile<KrakenPreviewToVPreviewDtoProfile>();
                c.AddProfile<KrakenResolutionsToResolutionsDtoProfile>();
                c.AddProfile<KrakenThumbnailsToThumbnailsDtoProfile>();
                c.AddProfile<KrakenThumbnailToThumbnailDtoProfile>();

            });
            return services;
        }

        /// <summary>
        /// Configuring TwitchApis, passing "Client IDs" and other things
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        internal static IServiceCollection ConfigureTwitchApisSection(this IServiceCollection services, IConfiguration configuration)
        {
            String sectionName = @"TwitchApis";
            IConfigurationSection twitchApisSection = configuration.GetSection(sectionName);
            if (!twitchApisSection.Exists())
            {
                throw new RequiredConfigurationSectionNotEstablishedException(sectionName);
            }
            String helixSectionName = $@"{sectionName}:{nameof(ApiSettings.HelixSettings)}";
            IConfigurationSection helixSection = configuration.GetSection(helixSectionName);

            String krakenSectionName = $@"{sectionName}:{nameof(ApiSettings.KrakenSettings)}";
            IConfigurationSection krakenSection = configuration.GetSection(krakenSectionName);

            String usherSectionName = $@"{sectionName}:{nameof(ApiSettings.UsherSettings)}";
            IConfigurationSection usherSection = configuration.GetSection(usherSectionName);

            services.Configure<ApiSettings>(twitchApisSection);
            services.Configure<UsherSettings>(usherSection);
            services.Configure<KrakenSettings>(krakenSection);
            services.Configure<HelixSettings>(helixSection);

            return services;
        }


        internal static IServiceCollection ConfigureJsonSerializerOptions(this IServiceCollection services, JsonSerializerSettings jsonSerializerSettings = null)
        {
            jsonSerializerSettings ??= new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { new StringEnumConverter() }

            };
            services.AddSingleton(jsonSerializerSettings.GetType(), jsonSerializerSettings);
            return services;

        }
        internal static IServiceCollection ConfigureApiContainers(this IServiceCollection services)
        {
            services.AddSingleton<KrakenApiConfigurationContainer>();
            services.AddSingleton<HelixApiConfigurationContainer>();
            return services;
        }
    }
}
