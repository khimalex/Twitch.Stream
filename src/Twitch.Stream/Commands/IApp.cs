using System.Threading;
using System.Threading.Tasks;

namespace Twitch.Stream.Commands
{
   internal interface IApp
   {
      Task RunAsync(CancellationToken token = default);
   }
}
