using Microservices.Adapters.ISignalR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Microservices.SignalR
{
    public class SignalRHub : Hub<IHubProxy>
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Send(string message)
        {
            await Clients.All.Receive(message);
            //await Task.Run(() =>
            //{
            //    //var model = JsonConvert.DeserializeObject<SignalRModel<dynamic>>(message);
            //    Clients.All.Receive(message);
            //});
        }
    }
}
