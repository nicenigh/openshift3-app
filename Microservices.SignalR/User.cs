using System;

namespace Microservices.SignalR
{
    public class User
    {
        public string PublicToken { get; set; }
        public string PrivateToken { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public bool Online { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime ConnectTime { get; set; }
    }
}
