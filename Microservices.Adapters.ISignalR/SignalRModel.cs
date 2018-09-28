using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Adapters.ISignalR
{
    public class SignalRModel<T> where T : class
    {
        public SignalRModel(string sender, string receiver, T content, bool durable = false)
        {
            this.Sender = sender;
            this.Receiver = new string[] { receiver };
            this.Content = content;
            this.Durable = durable;
            this.Type = typeof(T).Name;
            this.SendTime = DateTime.Now;
        }
        public SignalRModel(string sender, string[] receiver, T content, bool durable = false)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Content = content;
            this.Durable = durable;
            this.Type = typeof(T).Name;
            this.SendTime = DateTime.Now;
        }
        public SignalRModel(string receiver, T content, bool durable = false)
        {
            this.Sender = null;
            this.Receiver = new string[] { receiver };
            this.Content = content;
            this.Durable = durable;
            this.Type = typeof(T).Name;
            this.SendTime = DateTime.Now;
        }
        public SignalRModel(string[] receiver, T content, bool durable = false)
        {
            this.Sender = null;
            this.Receiver = receiver;
            this.Content = content;
            this.Durable = durable;
            this.Type = typeof(T).Name;
            this.SendTime = DateTime.Now;
        }
        public string Sender { get; set; }
        public string[] Receiver { get; set; }
        public T Content { get; set; }
        public string Type { get; set; }
        public DateTime SendTime { get; set; }
        public bool Durable { get; set; }
    }
}
