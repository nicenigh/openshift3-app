using Microservices.Adapters.ISignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.SignalR
{
    public class SignalRHub : Hub<IHubProxy>
    {
        public override Task OnConnectedAsync()
        {
            var token = Guid.NewGuid().ToString("N");
            UserCache.List.Add(new User
            {
                ConnectionId = Context.ConnectionId,
                PrivateToken = token,
                PublicToken = Guid.NewGuid().ToString("N"),
                ConnectTime = DateTime.Now,
                Online = true
            });
            Clients.Client(Context.ConnectionId).Connected(token);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = UserCache.List.FirstOrDefault(en => en.ConnectionId == Context.ConnectionId);
            if (user != null)
                UserCache.List.Remove(user);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Send(SignalRModel model)
        {
            model.Sender = UserCache.List.FirstOrDefault(en => en.ConnectionId == Context.ConnectionId).Name;
            List<string> connIds;
            if (model.Receiver.Length > 0)
                connIds = model.Receiver.Select(en => UserCache.List.FirstOrDefault(u => u.PublicToken == en))
                   .Where(en => en?.Online ?? false).Select(en => en.ConnectionId).ToList();
            else
                connIds = UserCache.List.Where(en => en.Online).Select(en => en.ConnectionId).ToList();
            model.Receiver = null;
            if (connIds.Count > 0)
                await Clients.Clients(connIds).Receive(model);
        }

        //public async Task Send(string message)
        //{
        //    await Clients.All.Receive(message);
        //    //await Task.Run(() =>
        //    //{
        //    //    //var model = JsonConvert.DeserializeObject<SignalRModel<dynamic>>(message);
        //    //    Clients.All.Receive(message);
        //    //});
        //}
    }
}
