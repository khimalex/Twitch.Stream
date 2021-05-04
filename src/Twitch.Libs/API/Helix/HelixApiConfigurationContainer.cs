using System;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Twitch.Libs.API.Helix
{
    public class HelixApiConfigurationContainer
    {
        public HelixApiConfigurationContainer(IOptions<ApiSettings> options, IMapper mapper, JsonSerializerSettings jsonSerializerSettings)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            JsonSerializerSettings = jsonSerializerSettings;
        }
        public IOptions<ApiSettings> Options { get; }
        public IMapper Mapper { get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }

    }
}
