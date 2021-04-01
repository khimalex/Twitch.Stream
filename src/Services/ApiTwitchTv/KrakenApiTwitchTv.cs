using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.KrakenModels;

namespace Twitch.Stream.Services.ApiTwitchTv
{
   public class KrakenApiTwitchTv : IApiTwitchTv
   {
      //private readonly String _channelsAuthStringFormat = @"/api/channels/{0}/access_token";
      private readonly String _twitchGQL = @"https://gql.twitch.tv/gql";
      private readonly String _vodsAuthStringFormat = @"/api/vods/{0}/access_token";

      private readonly String _userInfoStringFormat = @"/kraken/users?login={0}";
      private readonly String _userVideosInfoStringFormat = @"/kraken/channels/{0}/videos?limit={1}&broadcast_type={2}";
      private readonly String _videoInfoStringFormat = @"/kraken/videos/{0}";



      private readonly HttpClient _client;
      private readonly Appsettings _options;
      private readonly IMapper _mapper;

      public KrakenApiTwitchTv(HttpClient client, IOptions<Appsettings> options, IMapper mapper)
      {
         _client = client;
         _client.BaseAddress = new Uri(@"https://api.twitch.tv");
         //_client.DefaultRequestHeaders.Add("Client-ID", options.Value.ClientID);
         _client.DefaultRequestHeaders.Add("Accept", @"application/vnd.twitchtv.v5+json");

         _options = options.Value;
         _mapper = mapper;
      }
      public async Task<TwitchAuthDto> GetChannelTwitchAuthAsync(String channelName)
      {
         if (String.IsNullOrEmpty(channelName) || String.IsNullOrWhiteSpace(channelName))
         {
            throw new ArgumentException("Название канала не может быть пустым", nameof(channelName));
         }

         //   var query = @"query PlaybackAccessToken_Template($login: String!, $isLive: Boolean!, $vodID: ID!, $isVod: Boolean!, $playerType: String!) {  streamPlaybackAccessToken(channelName: $login, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isLive) {    value    signature    __typename  }  videoPlaybackAccessToken(id: $vodID, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isVod) {    value    signature    __typename  }}";

         //   var postData = $@"{{
         //   ""operationName"": ""PlaybackAccessToken_Template"",
         //   ""query"": ""{query}"",
         //      ""variables"": {{
         //          ""isLive"": true,
         //          ""login"": ""{channelName}"",
         //          ""isVod"": false,
         //          ""vodID"": """",
         //          ""playerType"": ""site"",
         //          ""show_ads"":false
         //      }}
         //}}";

         var postData = BuildTwitchGqlQuery(channelName: channelName);

         var request = new HttpRequestMessage(new HttpMethod("POST"), _twitchGQL);
         request.Content = new StringContent(postData);
         request.Headers.TryAddWithoutValidation("Client-ID", _options.ClientIDWeb);
         var response = await _client.SendAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            var failResponse = await response.Content.ReadAsStringAsync();
            throw new Exception($"Не удалось получить данные для канала '{channelName}', ответ Twitch '{failResponse}'");
         }

         var successResponse = await response.Content.ReadAsStringAsync();
         var twitchAuth = JsonConvert.DeserializeObject<TwitchAuth>(successResponse);

         return _mapper.Map<TwitchAuthDto>(twitchAuth);
      }

      public async Task<UsersDto> GetUserInfoAsync(String channelName)
      {
         if (String.IsNullOrEmpty(channelName))
         {
            throw new ArgumentException("Название канала не может быть пустым", nameof(channelName));
         }

         var request = new HttpRequestMessage(new HttpMethod("GET"), String.Format(_userInfoStringFormat, channelName));
         request.Headers.TryAddWithoutValidation("Client-ID", _options.ClientID);

         //String request = String.Format(_userInfoStringFormat, channelName);
         var response = await _client.SendAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            var responseBody = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($@"Не найден пользователь '{channelName}'. Статус ошибки: {(Int32)response.StatusCode,15}. Ответ Twitch: {responseBody}");
         }

         String resultString = await response.Content.ReadAsStringAsync();
         var users = JsonConvert.DeserializeObject<Users>(resultString);

         var result = _mapper.Map<UsersDto>(users);
         return result;

         //throw new System.NotImplementedException();
      }

      public async Task<VideosDto> GetUserVideosAsync(String userId, Int32 first = 100, String type = "archive")
      {

         if (String.IsNullOrEmpty(userId))
         {
            throw new ArgumentException("Идентификатор пользователя не может быть пустым", nameof(userId));
         }

         var request = new HttpRequestMessage(new HttpMethod("GET"), String.Format(_userVideosInfoStringFormat, userId, first, type));
         request.Headers.TryAddWithoutValidation("Client-ID", _options.ClientID);


         //String request = String.Format(_userVideosInfoStringFormat, userId, first, type);
         var response = await _client.SendAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            throw new HttpRequestException($@"Видео пользователя с идентификатором '{userId}' не загружены. Статус ошибки: {(Int32)response.StatusCode,15}.");
         }
         String str = await response.Content.ReadAsStringAsync();
         var videos = JsonConvert.DeserializeObject<Videos>(str);
         var dto = _mapper.Map<VideosDto>(videos);
         return dto;
      }

      public async Task<VideosDto> GetVideoInfoAsync(String videoId)
      {
         if (String.IsNullOrEmpty(videoId))
         {
            throw new ArgumentException("Идентификатор видео не может быть пустым", nameof(videoId));
         }

         var request = new HttpRequestMessage(new HttpMethod("GET"), String.Format(_videoInfoStringFormat, videoId));
         request.Headers.TryAddWithoutValidation("Client-ID", _options.ClientIDWeb);


         var response = await _client.SendAsync(request);


         //String request = String.Format(_videoInfoStringFormat, videoId);
         //var response = await _client.GetAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            throw new HttpRequestException($@"Видео '{videoId}' не загружено. Статус ошибки: '{(Int32)response.StatusCode}'.");
         }
         String str = await response.Content.ReadAsStringAsync();

         var video = JsonConvert.DeserializeObject<Video>(str);
         var videos = new Videos() { _total = 1, videos = new List<Video> { video } };
         var dto = _mapper.Map<VideosDto>(videos);
         return dto;

      }

      public async Task<TwitchAuthDto> GetVodTwitchAuthAsync(String videoId)
      {


         if (String.IsNullOrEmpty(videoId))
         {
            throw new ArgumentException("Идентификатор видео не может быть пустым", nameof(videoId));
         }

         //   var query = @"query PlaybackAccessToken_Template($login: String!, $isLive: Boolean!, $vodID: ID!, $isVod: Boolean!, $playerType: String!) {  streamPlaybackAccessToken(channelName: $login, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isLive) {    value    signature    __typename  }  videoPlaybackAccessToken(id: $vodID, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isVod) {    value    signature    __typename  }}";

         //   var postData = $@"{{
         //   ""operationName"": ""PlaybackAccessToken_Template"",
         //   ""query"": ""{query}"",
         //      ""variables"": {{
         //          ""isLive"": false,
         //          ""login"": """",
         //          ""isVod"": true,
         //          ""vodID"": ""{videoId}"",
         //          ""playerType"": ""site""
         //      }}
         //}}";

         var postData = BuildTwitchGqlQuery(videoId: videoId);

         var request1 = new HttpRequestMessage(new HttpMethod("POST"), _twitchGQL);
         request1.Content = new StringContent(postData);
         request1.Headers.TryAddWithoutValidation("Client-ID", _options.ClientIDWeb);
         var response1 = await _client.SendAsync(request1);
         if (!response1.IsSuccessStatusCode)
         {
            var resp = await response1.Content.ReadAsStringAsync();
            throw new Exception($"Не удалось получить данные для видео '{videoId}'");
         }
         var resp1 = await response1.Content.ReadAsStringAsync();

         var twitchAuth = JsonConvert.DeserializeObject<TwitchAuth>(resp1);

         return _mapper.Map<TwitchAuthDto>(twitchAuth);
      }


      private string BuildTwitchGqlQuery(String channelName = null, String videoId = null)
      {
         var query = @"query PlaybackAccessToken_Template($login: String!, $isLive: Boolean!, $vodID: ID!, $isVod: Boolean!, $playerType: String!) {  streamPlaybackAccessToken(channelName: $login, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isLive) {    value    signature    __typename  }  videoPlaybackAccessToken(id: $vodID, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isVod) {    value    signature    __typename  }}";

         var postData = $@"{{
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
