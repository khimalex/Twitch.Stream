using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Twitch.Libs.API.Kraken.Models;
using Twitch.Libs.Exceptions.API;
using Twitch.Libs.Serialization;
using Twitch.Libs.Dto;
using System.Linq;

namespace Twitch.Libs.API.Kraken
{
    public class KrakenApiTwitchTv : IApiTwitchTv
    {
        private readonly HttpClient _client;
        private readonly KrakenSettings _options;
        private readonly IMapper _mapper;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public KrakenApiTwitchTv(HttpClient client, KrakenApiConfigurationContainer krakenApiConfiguration)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", @"application/vnd.twitchtv.v5+json");

            _options = krakenApiConfiguration.Options.Value;
            _mapper = krakenApiConfiguration.Mapper;
            _jsonSerializerSettings = krakenApiConfiguration.JsonSerializerSettings;
        }
        public async Task<TwitchAuthDto> GetChannelTwitchAuthAsync(String channelName)
        {
            if (String.IsNullOrEmpty(channelName) || String.IsNullOrWhiteSpace(channelName))
            {
                throw new ArgumentException("Название канала не может быть пустым", nameof(channelName));
            }

            String postData = BuildTwitchGqlQuery(channelName: channelName);
            using var content = new StringContent(postData);
            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Post,
                requestUri: _options.TwitchGQL,
                content: content,
                headers: new Dictionary<String, String>
                {
                    {"Client-ID", _options.ClientIDWeb}
                }
            );

            using HttpResponseMessage response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
                throw new TwitchApiResponseException($"Could not receive data for channel: '{channelName}'", failResponse);
            }

            TwitchAuth twitchAuth = await response.Content.ReadFromJsonAsync<TwitchAuth>(_jsonSerializerSettings);

            return _mapper.Map<TwitchAuthDto>(twitchAuth);
        }

        public async Task<UsersDto> GetChannelInfoAsync(String channelName)
        {
            if (String.IsNullOrEmpty(channelName))
            {
                throw new ArgumentException("Channel name MUST be established!", nameof(channelName));
            }

            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Get,
                requestUri: $@"{_options.Api}/users?login={channelName}",
                headers: new Dictionary<String, String>
                {
                    {"Client-ID", _options.ClientID}
                }

            );

            using HttpResponseMessage response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
                throw new TwitchApiResponseException($"Could not receive data for channel: '{channelName}'.", failResponse);
            }

            Users users = await response.Content.ReadFromJsonAsync<Users>(_jsonSerializerSettings);

            UsersDto result = _mapper.Map<UsersDto>(users);
            return result;

        }

        public async Task<VideosDto> GetChannelVideosAsync(String channelName, Int32 first = 100, String type = "archive")
        {

            if (String.IsNullOrEmpty(channelName))
            {
                throw new ArgumentException("Channel Name MUST be provided!", nameof(channelName));
            }
            UsersDto userInfo = await GetChannelInfoAsync(channelName);
            if (!userInfo.Users.Any())
            {
                throw new Exception($"Channel '{channelName}' not found!");
            }

            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Get,
                requestUri: $@"{_options.Api}/channels/{userInfo.Users.First().Id}/videos?limit={first}&broadcast_type={type}",
                headers: new Dictionary<String, String>
                {
                    {"Client-ID", _options.ClientID}
                }

            );
            using HttpResponseMessage response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
                throw new TwitchApiResponseException($@"Could not receive Video for User Id '{channelName}'.", failResponse);
            }


            Videos videos = await response.Content.ReadFromJsonAsync<Videos>(_jsonSerializerSettings);

            return _mapper.Map<VideosDto>(videos);
        }

        public async Task<VideosDto> GetVideoInfoAsync(String videoId)
        {
            if (String.IsNullOrEmpty(videoId))
            {
                throw new ArgumentException("Идентификатор видео не может быть пустым", nameof(videoId));
            }

            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Get,
                requestUri: $@"{_options.Api}/videos/{videoId}",
                headers: new Dictionary<String, String>
                {
                    {"Client-ID", _options.ClientID}
                }
            );
            using HttpResponseMessage response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
                throw new TwitchApiResponseException($@"Could not download video by Id: '{videoId}'.", failResponse);
            }

            Video video = await response.Content.ReadFromJsonAsync<Video>(_jsonSerializerSettings);

            var videos = new Videos() { _total = 1, videos = new List<Video> { video } };

            return _mapper.Map<VideosDto>(videos);

        }

        public async Task<TwitchAuthDto> GetVodTwitchAuthAsync(String videoId)
        {
            if (String.IsNullOrEmpty(videoId))
            {
                throw new ArgumentException("Video Id MUST be provided!", nameof(videoId));
            }

            String postData = BuildTwitchGqlQuery(videoId: videoId);
            using var content = new StringContent(postData);
            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Post,
                requestUri: _options.TwitchGQL,
                content: content,
                headers: new Dictionary<String, String>
                {
                    {"Client-ID", _options.ClientIDWeb}
                }
            );
            using HttpResponseMessage response1 = await _client.SendAsync(request);
            if (!response1.IsSuccessStatusCode)
            {
                _ = await response1.Content.ReadAsStringAsync();
                throw new Exception($"Couldn't receive video '{videoId}'");
            }

            TwitchAuth twitchAuth = await response1.Content.ReadFromJsonAsync<TwitchAuth>(_jsonSerializerSettings);

            return _mapper.Map<TwitchAuthDto>(twitchAuth);
        }


        private String BuildTwitchGqlQuery(String channelName = null, String videoId = null)
        {
            String query = @"query PlaybackAccessToken_Template($login: String!, $isLive: Boolean!, $vodID: ID!, $isVod: Boolean!, $playerType: String!) {  streamPlaybackAccessToken(channelName: $login, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isLive) {    value    signature    __typename  }  videoPlaybackAccessToken(id: $vodID, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isVod) {    value    signature    __typename  }}";

            String postData = $@"{{
         ""operationName"": ""PlaybackAccessToken_Template"",
         ""query"": ""{query}"",
            ""variables"": {{
                ""isLive"": {(!String.IsNullOrEmpty(channelName)).ToString().ToLowerInvariant()},
                ""login"": ""{channelName?.ToString().ToLowerInvariant()}"",
                ""isVod"": {(!String.IsNullOrEmpty(videoId)).ToString().ToLowerInvariant()},
                ""vodID"": ""{videoId}"",
                ""playerType"": ""site""
            }}
      }}";
            return postData;
        }
    }
}
