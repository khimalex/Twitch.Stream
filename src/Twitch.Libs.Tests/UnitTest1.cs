using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Twitch.Libs.API;
using Xunit;

namespace Twitch.Libs.Tests
{
    public class UnitTest1
    {
        private readonly ApiSettings _configuration;

        public UnitTest1()
        {
            String settingsPath = String.Empty;

#if DEBUG
            settingsPath = "ApiSettings.Local.json";
#else
            settingsPath = "ApiSettings.Git.json";
#endif


            System.Console.BackgroundColor = ConsoleColor.Blue;
            System.Console.WriteLine($"SettingPath:  {settingsPath}");
            System.Console.ResetColor();

            _configuration = JsonConvert.DeserializeObject<ApiSettings>(File.ReadAllText(settingsPath));
        }
        [Fact]
        public async Task TestAsync()
        {

            String baseAddress = @"https://id.twitch.tv/oauth2/token";

            var form = new Dictionary<String, String>
                           {
                               {"grant_type", "client_credentials"},
                               {"client_id", _configuration.ClientID},
                               {"client_secret", _configuration.ClientSecret},
                           };

            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            HttpResponseMessage response = await client.PostAsync(baseAddress, new FormUrlEncodedContent(form));

            Assert.True(response.IsSuccessStatusCode);
            // if (!response.IsSuccessStatusCode)
            // {
            //     _ = await response.Content.ReadAsStringAsync();
            // }
            // Token token = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
            /*
             * ������� ����� ����������.
             * Authenticate your app and allow it to access resources that it owns. Since app access tokens are not associated with a user, they cannot be used with endpoints that require user authentication.

   Some Twitch API endpoints require application authentication (not user authentication). If your application uses these endpoints, you need to generate an app access token. App access tokens get client credentials (not user credentials). They enable you to make secure API requests that are not on behalf of a specific user. Client credentials also may be used in place of client ID headers to securely identify your application.

   App access tokens expire after about 60 days, so you should check that your app access token is valid by submitting a request to the validation endpoint (see Validating Requests). If your token has expired, generate a new one.

   App access tokens are meant only for server-to-server API requests and should never be included in client code.
             */
        }

        internal class Token
        {
            [JsonProperty("access_token")]
            public String AccessToken { get; set; }

            [JsonProperty("token_type")]
            public String TokenType { get; set; }

            [JsonProperty("expires_in")]
            public Int32 ExpiresIn { get; set; }

            [JsonProperty("refresh_token")]
            public String RefreshToken { get; set; }
            [JsonProperty("scope")]
            public String Scope { get; set; }
        }
    }
}
