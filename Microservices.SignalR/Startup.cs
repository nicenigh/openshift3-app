﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Microservices.SignalR
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSignalR();
            services.AddSingleton<IServiceProvider, ServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
#if DEBUG
            var wwwroot = Path.Combine(@"C:\Projects\openshift3-app\Microservices.SignalR", "wwwroot");
#else
            var wwwroot = Environment.GetEnvironmentVariable("wwwroot");
            //var wwwroot = "/opt/app-root/src/Microservices.SignalR/wwwroot";
#endif
            Console.WriteLine($"Using wwwroot from: {wwwroot}");
            if (!Directory.Exists(wwwroot))
                Directory.CreateDirectory(wwwroot);
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(wwwroot)
                });

            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalRHub>("/signalr");
            });
            app.UseWebSockets();

            app.UseMvc();
        }
    }
}
