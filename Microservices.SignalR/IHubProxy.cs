using System.Threading.Tasks;

namespace Microservices.SignalR
{
    public interface IHubProxy
    {
        Task Receive(string message);
        Task Disconnect();
    }
}
