using System;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Twitch.Libs.API.Kraken
{
    public class KrakenApiConfigurationContainer
    {
        public KrakenApiConfigurationContainer(IOptions<KrakenSettings> options, IMapper mapper, JsonSerializerSettings jsonSerializerOptions)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            //Null Cheking not neccesary
            JsonSerializerSettings = jsonSerializerOptions;
        }

        public IOptions<KrakenSettings> Options { get; }
        public IMapper Mapper { get; }
        public JsonSerializerSettings JsonSerializerSettings { get; }
    }
}
