using Microservices.Base;

namespace Microservices.Adapters.ISignalR
{
    public interface ISignalRAdapter : IAdapter
    {
        void Send<T>(SignalRModel<T> model) where T : class;
    }
}
