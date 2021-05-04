using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Twitch.Libs.Serialization
{
    internal static class NewtonsoftExtensions
    {
        internal static async Task<T> ReadFromJsonAsync<T>(this HttpContent content, JsonSerializerSettings settings = default, CancellationToken token = default)
        {
            return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync(token), settings);
        }
    }
}
