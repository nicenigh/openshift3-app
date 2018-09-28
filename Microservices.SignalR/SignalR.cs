using Microservices.Builder;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Microservices.SignalR
{
    public static class SignalR
    {
        public static ServiceBuilder UseSignalR(this ServiceBuilder builder)
        {
            builder.ConfigureServices((b) =>
            {
                var host = WebHost.CreateDefaultBuilder().UseKestrel()
                     .UseStartup<Startup>();
                host.Build().Run();
            });
            return builder;
        }
    }
}
