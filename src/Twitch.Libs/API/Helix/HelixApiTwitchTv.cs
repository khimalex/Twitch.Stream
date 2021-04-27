using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Twitch.Libs.API.Helix.Models;
using Twitch.Libs.Exceptions.API;
using Twitch.Libs.Serialization;
using Twitch.Stream.Dto;

namespace Twitch.Libs.API.Helix
{
   public class HelixApiTwitchTv : IApiTwitchTv
   {
      private readonly HttpClient _client;
      private readonly IMapper _mapper;
      private readonly JsonSerializerSettings _jsonSerializerSettings;

      public HelixApiTwitchTv(HttpClient c, HelixApiConfigurationContainer helixApiConfigurationContainer)
      {
         c.BaseAddress = new Uri(helixApiConfigurationContainer.Options.Value.TwitchGQL);
         c.DefaultRequestHeaders.Add("Client-ID", helixApiConfigurationContainer.Options.Value.ClientIDWeb);
         _client = c;
         _mapper = helixApiConfigurationContainer.Mapper;
         _jsonSerializerSettings = helixApiConfigurationContainer.JsonSerializerSettings;
      }
      public async Task<TwitchAuthDto> GetChannelTwitchAuthAsync(String channelName)
      {
         if (String.IsNullOrEmpty(channelName))
         {
            throw new ArgumentException("Channel name MUST be established!", nameof(channelName));
         }

         String query = @"query PlaybackAccessToken_Template($login: String!, $isLive: Boolean!, $vodID: ID!, $isVod: Boolean!, $playerType: String!) {  streamPlaybackAccessToken(channelName: $login, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isLive) {    value    signature    __typename  }  videoPlaybackAccessToken(id: $vodID, params: {platform: \""web\"", playerBackend: \""mediaplayer\"", playerType: $playerType}) @include(if: $isVod) {    value    signature    __typename  }}";

         String postData = $@"{{
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

         HttpResponseMessage resp = await _client.PostAsync((String)null, new StringContent(postData));

         if (!resp.IsSuccessStatusCode)
         {
            CommonModels.ErrorResponse failResponse = await resp.Content.ReadFromJsonAsync<CommonModels.ErrorResponse>(_jsonSerializerSettings);
            throw new TwitchApiResponseException($"Could not receive data for channel'{channelName}'", failResponse);
         }
         String rsp = await resp.Content.ReadAsStringAsync();

         TwitchAuth twitchAuth = await resp.Content.ReadFromJsonAsync<TwitchAuth>(_jsonSerializerSettings);

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
