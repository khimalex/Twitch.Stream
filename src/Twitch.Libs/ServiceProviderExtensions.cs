using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Twitch.Libs.API;
using Twitch.Libs.API.Helix;
using Twitch.Libs.API.Kraken;
using Twitch.Libs.Exceptions.Configuration;
using Twitch.Libs.Profiles;

namespace Twitch.Libs
{
   /// <summary>
   /// Extensions for customized configurations of client services
   /// </summary>
   public static class ServiceProviderExtensions
   {

      public static IServiceCollection ConfigureTwitchLibs(this IServiceCollection services, IConfiguration configuration, JsonSerializerSettings jsonSerializerSettings = null)
      {
         services.ConfigureTwitchApisSection(configuration);
         services.ConfigureAutoMapper();
         services.ConfigureJsonSerializerOptions(jsonSerializerSettings);
         services.ConfigureApiContainers();

         return services;
      }

      private static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
      {
         //Don't need checking double registration, as i think...
         //Developers checked it already, see here:
         //https://github.com/AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection/blob/be2b8cdb0b52d92fe873f0a7d1baacdd9fe0e5aa/src/AutoMapper.Extensions.Microsoft.DependencyInjection/ServiceCollectionExtensions.cs#L103
         services.AddAutoMapper(c =>
         {
            c.AddProfile<HelixTwitchAuthToTwitchAuthDtoProfile>();
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
      private static IServiceCollection ConfigureTwitchApisSection(this IServiceCollection services, IConfiguration configuration)
      {
         String sectionName = @"TwitchApis";
         IConfigurationSection twitchApisSection = configuration.GetSection(sectionName);
         if (!twitchApisSection.Exists())
         {
            throw new RequiredConfigurationSectionNotEstablishedException(sectionName);
         }
         services.Configure<ApiSettings>(twitchApisSection);

         return services;
      }


      private static IServiceCollection ConfigureJsonSerializerOptions(this IServiceCollection services, JsonSerializerSettings jsonSerializerSettings)
      {
         jsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();

         jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

         jsonSerializerSettings.Converters.Add(new StringEnumConverter());

         services.AddSingleton(jsonSerializerSettings.GetType(), jsonSerializerSettings);

         return services;

      }
      private static IServiceCollection ConfigureApiContainers(this IServiceCollection services)
      {
         services.AddSingleton<KrakenApiConfigurationContainer>();
         services.AddSingleton<HelixApiConfigurationContainer>();
         return services;
      }
   }
}
