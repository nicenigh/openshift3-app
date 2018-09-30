using System;

namespace Microservices.Adapters.ISignalR
{
    public class SignalRModel
    {
        public string Sender { get; set; }
        public string[] Receiver { get; set; } = new string[] { };
        public string Content { get; set; }
        public string Type { get; set; }
        public string Time { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public bool Durable { get; set; } = false;
    }
}
