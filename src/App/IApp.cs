using System.Threading;
using System.Threading.Tasks;

namespace Twitch.Stream.App
{
   interface IApp
   {
      Task RunAsync(CancellationToken token = default);
   }
}
