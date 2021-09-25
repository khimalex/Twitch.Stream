using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Dto;
using Twitch.Libs.Exceptions.API;
using Twitch.Libs.Serialization;

namespace Twitch.Libs.API.Helix
{
    public class HelixApiTwitchTv : IApiTwitchTv
    {
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly HelixSettings _options;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private AppOAuthToken _appOAuthToken;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public HelixApiTwitchTv(HttpClient c, HelixApiConfigurationContainer helixApiConfigurationContainer)
        {
            //c.BaseAddress = new Uri(helixApiConfigurationContainer.Options.Value.TwitchGQL);
            //c.DefaultRequestHeaders.Add("Client-ID", helixApiConfigurationContainer.Options.Value.ClientIDWeb);
            _client = c;
            _options = helixApiConfigurationContainer.Options.Value;
            _mapper = helixApiConfigurationContainer.Mapper;
            _jsonSerializerSettings = helixApiConfigurationContainer.JsonSerializerSettings;
        }
        public async Task<TwitchAuthDto> GetChannelTwitchAuthAsync(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentException("Channel name MUST be established!", nameof(channelName));
            }

            string postData = BuildTwitchGqlQuery(channelName);
            using var requestBody = new StringContent(postData);
            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Post,
                requestUri: _options.TwitchGQL,
                content: requestBody,
                headers: new Dictionary<string, string>()
                {
                    { "Client-ID", _options.ClientIDWeb }
                }
            );

            using HttpResponseMessage response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
                throw new TwitchApiResponseException($"Could not receive data for channel'{channelName}'", failResponse);
            }

            _ = await response.Content.ReadAsStringAsync();

            TwitchAuth twitchAuth = await response.Content.ReadFromJsonAsync<TwitchAuth>(_jsonSerializerSettings);

            return _mapper.Map<TwitchAuthDto>(twitchAuth);
        }

        public async ValueTask<AppOAuthToken> GetAppOAuthAccessTokenAsync()
        {
            if (_appOAuthToken is null)
            {
                try
                {
                    await _semaphore.WaitAsync();
                    if (_appOAuthToken is null)
                    {
                        var form = new Dictionary<string, string>
                        {
                            {"grant_type", "client_credentials"},
                            {"client_id", _options.ClientID},
                            {"client_secret", _options.ClientSecret},
                        };
                        using var requestBody = new FormUrlEncodedContent(form);
                        using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
                        (
                            method: HttpMethod.Post,
                            requestUri: $"{_options.AuthorizationAndAuthentification}/oauth2/token",
                            content: requestBody,
                            headers: new Dictionary<string, string>()
                            {
                                { "Client-ID", _options.ClientID }
                            }
                        );

                        using HttpResponseMessage response = await _client.SendAsync(request);
                        // String str = await response.Content.ReadAsStringAsync();
                        AppOAuthToken token = await response.Content.ReadFromJsonAsync<AppOAuthToken>(_jsonSerializerSettings);
                        _appOAuthToken = token;
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return _appOAuthToken;
        }
        public async Task<UsersDto> GetChannelInfoAsync(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentException("Channel name MUST be established!", nameof(channelName));
            }

            AppOAuthToken token = await GetAppOAuthAccessTokenAsync();

            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Get,
                requestUri: $@"{_options.Api}/users?login={channelName}",
                headers: new Dictionary<string, string>()
                {
                    { "Client-ID", _options.ClientID },
                    { "Authorization", $"Bearer {token.AccessToken}" },
                }
            );
            using HttpResponseMessage response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {

                CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
                throw new TwitchApiResponseException($"Could not receive data for channel: '{channelName}'.", failResponse);
            }

            Users users = await response.Content.ReadFromJsonAsync<Users>(_jsonSerializerSettings);
            return _mapper.Map<UsersDto>(users);
        }

        public async Task<VideosDto> GetChannelVideosAsync(string channelName, int first = 10, string type = "archive")
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentException("Channel Name MUST be provided!", nameof(channelName));
            }

            UsersDto userInfo = await GetChannelInfoAsync(channelName);
            if (!userInfo.UserList.Any())
            {
                throw new Exception($"Channel '{channelName}' not found!");
            }

            AppOAuthToken token = await GetAppOAuthAccessTokenAsync();
            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Get,
                requestUri: $@"{_options.Api}/videos?user_id={userInfo.UserList.First().Id}&first={first}&type={type}",
                headers: new Dictionary<string, string>
                {
                    {"Client-ID", _options.ClientID},
                    { "Authorization", $"Bearer {token.AccessToken}" }

                }

            );
            using HttpResponseMessage response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
                throw new TwitchApiResponseException($"Could not receive videos for channel: '{channelName}'.", failResponse);
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            Videos videos = await response.Content.ReadFromJsonAsync<Videos>(_jsonSerializerSettings);
            VideosDto videoDtos = _mapper.Map<VideosDto>(videos);
            return videoDtos;

        }

        public async Task<VideosDto> GetVideoInfoAsync(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
            {
                throw new ArgumentException("Идентификатор видео не может быть пустым", nameof(videoId));
            }

            AppOAuthToken token = await GetAppOAuthAccessTokenAsync();
            videoId = new string(new string(videoId.Where(c => char.IsDigit(c)).ToArray()));
            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Get,
                requestUri: $@"{_options.Api}/videos?id={videoId}",
                headers: new Dictionary<string, string>
                {
                    {"Client-ID", _options.ClientID},
                    { "Authorization", $"Bearer {token.AccessToken}" }

                }
            );
            using HttpResponseMessage response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
                throw new TwitchApiResponseException($@"Could not download video by Id: '{videoId}'.", failResponse);
            }

            Videos videos = await response.Content.ReadFromJsonAsync<Videos>(_jsonSerializerSettings);
            VideosDto dto = _mapper.Map<VideosDto>(videos);

            return dto;

        }

        public async Task<TwitchAuthDto> GetVodTwitchAuthAsync(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
            {
                throw new ArgumentException("Video Id MUST be provided!", nameof(videoId));
            }

            string postData = BuildTwitchGqlQuery(videoId: videoId);
            using var content = new StringContent(postData);
            using HttpRequestMessage request = IApiTwitchTvHelpers.BuildRequest
            (
                method: HttpMethod.Post,
                requestUri: _options.TwitchGQL,
                content: content,
                headers: new Dictionary<string, string>
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
            //var str = await response1.Content.ReadAsStringAsync();
            TwitchAuth twitchAuth = await response1.Content.ReadFromJsonAsync<TwitchAuth>(_jsonSerializerSettings);

            return _mapper.Map<TwitchAuthDto>(twitchAuth);

        }

        private string BuildTwitchGqlQuery(string channelName = null, string videoId = null)
        {
            string query = @"query PlaybackAccessToken_Template($login: String!, $isLive: Boolean!, $vodID: ID!, $isVod: Boolean!, $playerType: String!) {  streamPlaybackAccessToken(channelName: $login, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isLive) {    value    signature    __typename  }  videoPlaybackAccessToken(id: $vodID, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isVod) {    value    signature    __typename  }}";

            string postData = $@"{{
         ""operationName"": ""PlaybackAccessToken_Template"",
         ""query"": ""{query}"",
            ""variables"": {{
                ""isLive"": {(!string.IsNullOrEmpty(channelName)).ToString().ToLowerInvariant()},
                ""login"": ""{channelName?.ToString().ToLowerInvariant()}"",
                ""isVod"": {(!string.IsNullOrEmpty(videoId)).ToString().ToLowerInvariant()},
                ""vodID"": ""{videoId}"",
                ""playerType"": ""site""
            }}
      }}";
            return postData;
        }
    }
}
