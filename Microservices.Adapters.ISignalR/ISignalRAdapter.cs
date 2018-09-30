using Microservices.Base;

namespace Microservices.Adapters.ISignalR
{
    public interface ISignalRAdapter : IAdapter
    {
        void Send(SignalRModel model);
    }
}
