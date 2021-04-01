using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Twitch.Stream.Dto;
using Twitch.Stream.Services.ApiTwitchTv.HelixModels;

namespace Twitch.Stream.Services.ApiTwitchTv
{
   class HelixApiTwitchTv : IApiTwitchTv
   {
      //private readonly String _channelsAuthStringFormat = @"/api/channels/{0}/access_token";
      private readonly HttpClient _client;
      private readonly IMapper _mapper;
      private readonly String _twitchGQL = @"https://gql.twitch.tv/gql";

      public HelixApiTwitchTv(HttpClient c, IOptions<Appsettings> options, IMapper mapper)
      {
         c.BaseAddress = new Uri(_twitchGQL);
         c.DefaultRequestHeaders.Add("Client-ID", options.Value.ClientIDWeb);
         //c.DefaultRequestHeaders.Add("Accept", @"application/vnd.twitchtv.v5+json");
         _client = c;
         _mapper = mapper;
      }
      public async Task<TwitchAuthDto> GetChannelTwitchAuthAsync(String channelName)
      {
         if (String.IsNullOrEmpty(channelName))
         {
            throw new ArgumentException("Название канала не может быть пустым", nameof(channelName));
         }

         var query = @"query PlaybackAccessToken_Template($login: String!, $isLive: Boolean!, $vodID: ID!, $isVod: Boolean!, $playerType: String!) {  streamPlaybackAccessToken(channelName: $login, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isLive) {    value    signature    __typename  }  videoPlaybackAccessToken(id: $vodID, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isVod) {    value    signature    __typename  }}";

         var postData = $@"{{
         ""operationName"": ""PlaybackAccessToken_Template"",
         ""query"": ""{query}"",
            ""variables"": {{
                ""isLive"": true,
                ""login"": ""{channelName}"",
                ""isVod"": false,
                ""vodID"": """",
                ""playerType"": ""site""
            }}
      }}";

         var resp = await _client.PostAsync((String)null, new StringContent(postData));

         if (!resp.IsSuccessStatusCode)
         {
            var rsp1 = await resp.Content.ReadAsStringAsync();
            throw new Exception($"Не удалось получить данные для канала '{channelName}'");
         }
         var rsp = await resp.Content.ReadAsStringAsync();

         var twitchAuth = JsonConvert.DeserializeObject<TwitchAuth>(rsp);

         return _mapper.Map<TwitchAuthDto>(twitchAuth);
      }

      public Task<UsersDto> GetUserInfoAsync(String channelName)
      {
         throw new NotImplementedException();
      }

      public Task<VideosDto> GetUserVideosAsync(String userId, Int32 first = 10, String type = "archive")
      {
         throw new NotImplementedException();
      }

      public Task<VideosDto> GetVideoInfoAsync(String videoId)
      {
         throw new NotImplementedException();
      }

      public Task<TwitchAuthDto> GetVodTwitchAuthAsync(String videoId)
      {
         throw new NotImplementedException();
      }
   }
}
