using Microservices.Builder;
using Microservices.SignalR;
using System;

namespace openshift3_app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            new ServiceBuilder().UseSignalR().Run();
        }
    }
}
