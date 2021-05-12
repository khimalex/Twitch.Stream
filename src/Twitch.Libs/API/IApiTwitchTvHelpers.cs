using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Twitch.Libs.API
{
    public class IApiTwitchTvHelpers
    {
        internal static HttpRequestMessage BuildRequest(
            HttpMethod method,
            String requestUri,
            HttpContent content = default,
            Dictionary<String, String> headers = default)
        {
            var result = new HttpRequestMessage()
            {
                RequestUri = new Uri(requestUri),
                Method = method,
                Content = content,
            };

            headers?.ToList().ForEach(h => result.Headers.TryAddWithoutValidation(h.Key, h.Value));

            return result;
        }
    }
}
