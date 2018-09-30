using Microservices.Adapters.ISignalR;
using System.Threading.Tasks;

namespace Microservices.SignalR
{
    public interface IHubProxy
    {
        Task Connected(string token);
        Task Disconnect(string message);
        Task Receive(SignalRModel model);
    }
}
