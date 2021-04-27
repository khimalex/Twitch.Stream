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
using Twitch.Stream.Dto;

namespace Twitch.Libs.API.Kraken
{
   public class KrakenApiTwitchTv : IApiTwitchTv
   {
      //Why this strings here? Because all of tightly affiliated to the Kraken API.
      private readonly String _userInfoStringFormat = @"/kraken/users?login={0}";
      private readonly String _userVideosInfoStringFormat = @"/kraken/channels/{0}/videos?limit={1}&broadcast_type={2}";
      private readonly String _videoInfoStringFormat = @"/kraken/videos/{0}";



      private readonly HttpClient _client;
      private readonly ApiSettings _options;
      private readonly IMapper _mapper;
      private readonly JsonSerializerSettings _jsonSerializerSettings;

      public KrakenApiTwitchTv(HttpClient client, KrakenApiConfigurationContainer krakenApiConfiguration)
      {
         _client = client;
         //Base Kraken API Url Only for Kraken API
         _client.BaseAddress = new Uri(@"https://api.twitch.tv");
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

         var request = new HttpRequestMessage(new HttpMethod("POST"), _options.TwitchGQL)
         {
            Content = new StringContent(postData)
         };
         request.Headers.TryAddWithoutValidation("Client-ID", _options.ClientIDWeb);
         HttpResponseMessage response = await _client.SendAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
            throw new TwitchApiResponseException($"Could not receive data for channel: '{channelName}'", failResponse);
         }

         TwitchAuth twitchAuth = await response.Content.ReadFromJsonAsync<TwitchAuth>(_jsonSerializerSettings);

         return _mapper.Map<TwitchAuthDto>(twitchAuth);
      }

      public async Task<UsersDto> GetUserInfoAsync(String channelName)
      {
         if (String.IsNullOrEmpty(channelName))
         {
            throw new ArgumentException("Channel name MUST be established!", nameof(channelName));
         }

         var request = new HttpRequestMessage(new HttpMethod("GET"), String.Format(_userInfoStringFormat, channelName));
         request.Headers.TryAddWithoutValidation("Client-ID", _options.ClientID);

         //String request = String.Format(_userInfoStringFormat, channelName);
         HttpResponseMessage response = await _client.SendAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
            throw new TwitchApiResponseException($"Could not receive data for channel: '{channelName}'.", failResponse);
         }

         Users users = await response.Content.ReadFromJsonAsync<Users>(_jsonSerializerSettings);

         UsersDto result = _mapper.Map<UsersDto>(users);
         return result;

         //throw new System.NotImplementedException();
      }

      public async Task<VideosDto> GetUserVideosAsync(String userId, Int32 first = 100, String type = "archive")
      {

         if (String.IsNullOrEmpty(userId))
         {
            throw new ArgumentException("User Id MUST be provided!", nameof(userId));
         }

         var request = new HttpRequestMessage(new HttpMethod("GET"), String.Format(_userVideosInfoStringFormat, userId, first, type));
         request.Headers.TryAddWithoutValidation("Client-ID", _options.ClientID);


         //String request = String.Format(_userVideosInfoStringFormat, userId, first, type);
         HttpResponseMessage response = await _client.SendAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            CommonModels.ErrorResponse failResponse = await response.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
            throw new TwitchApiResponseException($@"Could not receive Video for User Id '{userId}'.", failResponse);
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

         var request = new HttpRequestMessage(new HttpMethod("GET"), String.Format(_videoInfoStringFormat, videoId));
         request.Headers.TryAddWithoutValidation("Client-ID", _options.ClientIDWeb);


         HttpResponseMessage response = await _client.SendAsync(request);


         //String request = String.Format(_videoInfoStringFormat, videoId);
         //var response = await _client.GetAsync(request);
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
            throw new ArgumentException("Идентификатор видео не может быть пустым", nameof(videoId));
         }

         String postData = BuildTwitchGqlQuery(videoId: videoId);

         var request1 = new HttpRequestMessage(new HttpMethod("POST"), _options.TwitchGQL)
         {
            Content = new StringContent(postData)
         };
         request1.Headers.TryAddWithoutValidation("Client-ID", _options.ClientIDWeb);
         HttpResponseMessage response1 = await _client.SendAsync(request1);
         if (!response1.IsSuccessStatusCode)
         {
            String resp = await response1.Content.ReadAsStringAsync();
            throw new Exception($"Не удалось получить данные для видео '{videoId}'");
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
