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
      //private readonly String _vodsAuthStringFormat = @"/api/vods/{0}/access_token";

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

         String postData = BuildTwitchGqlQuery(channelName: channelName);

         var request = new HttpRequestMessage(new HttpMethod("POST"), _twitchGQL)
         {
            Content = new StringContent(postData)
         };
         request.Headers.TryAddWithoutValidation("Client-ID", _options.ClientIDWeb);
         HttpResponseMessage response = await _client.SendAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            String failResponse = await response.Content.ReadAsStringAsync();
            throw new Exception($"Не удалось получить данные для канала '{channelName}', ответ Twitch '{failResponse}'");
         }

         String successResponse = await response.Content.ReadAsStringAsync();
         TwitchAuth twitchAuth = JsonConvert.DeserializeObject<TwitchAuth>(successResponse);

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
         HttpResponseMessage response = await _client.SendAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            String responseBody = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($@"Не найден пользователь '{channelName}'. Статус ошибки: {(Int32)response.StatusCode,15}. Ответ Twitch: {responseBody}");
         }

         String resultString = await response.Content.ReadAsStringAsync();
         Users users = JsonConvert.DeserializeObject<Users>(resultString);

         UsersDto result = _mapper.Map<UsersDto>(users);
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
         HttpResponseMessage response = await _client.SendAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            throw new HttpRequestException($@"Видео пользователя с идентификатором '{userId}' не загружены. Статус ошибки: {(Int32)response.StatusCode,15}.");
         }
         String str = await response.Content.ReadAsStringAsync();
         Videos videos = JsonConvert.DeserializeObject<Videos>(str);
         VideosDto dto = _mapper.Map<VideosDto>(videos);
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


         HttpResponseMessage response = await _client.SendAsync(request);


         //String request = String.Format(_videoInfoStringFormat, videoId);
         //var response = await _client.GetAsync(request);
         if (!response.IsSuccessStatusCode)
         {
            throw new HttpRequestException($@"Видео '{videoId}' не загружено. Статус ошибки: '{(Int32)response.StatusCode}'.");
         }
         String str = await response.Content.ReadAsStringAsync();

         Video video = JsonConvert.DeserializeObject<Video>(str);
         var videos = new Videos() { _total = 1, videos = new List<Video> { video } };
         VideosDto dto = _mapper.Map<VideosDto>(videos);
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

         String postData = BuildTwitchGqlQuery(videoId: videoId);

         var request1 = new HttpRequestMessage(new HttpMethod("POST"), _twitchGQL)
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
         String resp1 = await response1.Content.ReadAsStringAsync();

         TwitchAuth twitchAuth = JsonConvert.DeserializeObject<TwitchAuth>(resp1);

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
