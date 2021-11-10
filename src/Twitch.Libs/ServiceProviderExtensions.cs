using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Twitch.Libs.API;
using Twitch.Libs.API.Helix;
using Twitch.Libs.API.Usher;
using Twitch.Libs.Exceptions.Configuration;
using Twitch.Libs.Profiles;

//Assembly of tests for this library MUST wil be named 'Twitch.Libs.Tests'.
[assembly: InternalsVisibleTo("Twitch.Libs.Tests")]
namespace Twitch.Libs;

/// <summary>
/// Extensions for customized configurations of client services
/// </summary>
public static class ServiceProviderExtensions
{

    public static IServiceCollection ConfigureTwitchLibs(this IServiceCollection services, IConfiguration configuration, JsonSerializerSettings jsonSerializerSettings = null)
    {
        _ = services.ConfigureAutoMapper()
        .ConfigureTwitchApisSection(configuration)
        .ConfigureJsonSerializerOptions(jsonSerializerSettings)
        .ConfigureApiContainers();

        return services;
    }

    internal static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
    {
        //Don't need checking double registration, as i think...
        //Developers checked it already, see here:
        //https://github.com/AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection/blob/be2b8cdb0b52d92fe873f0a7d1baacdd9fe0e5aa/src/AutoMapper.Extensions.Microsoft.DependencyInjection/ServiceCollectionExtensions.cs#L103
        _ = services.AddAutoMapper(c =>
        {
            c.AddProfile<HelixTwitchAuthToTwitchAuthDtoProfile>();
            c.AddProfile<HelixUsersToUsersDtoProfile>();
            c.AddProfile<HelixUserToUserDtoProfile>();
            c.AddProfile<HelixVideoToVideoDtoProfile>();
            c.AddProfile<HelixVideosToVideosDtoProfile>();
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
        string sectionName = @"TwitchApis";
        IConfigurationSection twitchApisSection = configuration.GetSection(sectionName);
        if (!twitchApisSection.Exists())
        {
            throw new RequiredConfigurationSectionNotEstablishedException(sectionName);
        }

        string helixSectionName = $@"{sectionName}:{nameof(ApiSettings.HelixSettings)}";
        IConfigurationSection helixSection = configuration.GetSection(helixSectionName);

        string usherSectionName = $@"{sectionName}:{nameof(ApiSettings.UsherSettings)}";
        IConfigurationSection usherSection = configuration.GetSection(usherSectionName);

        _ = services.Configure<ApiSettings>(twitchApisSection)
            .Configure<UsherSettings>(usherSection)
            .Configure<HelixSettings>(helixSection);

        return services;
    }

    internal static IServiceCollection ConfigureJsonSerializerOptions(this IServiceCollection services, JsonSerializerSettings jsonSerializerSettings = null)
    {
        jsonSerializerSettings ??= new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = { new StringEnumConverter() }

        };
        return services.AddSingleton(jsonSerializerSettings.GetType(), jsonSerializerSettings);

    }
    internal static IServiceCollection ConfigureApiContainers(this IServiceCollection services)
    {
        return services.AddSingleton<HelixApiConfigurationContainer>();
    }
}
