using Microsoft.AspNetCore.Builder;
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
            var wwwroot = Path.Combine(Environment.CurrentDirectory, "wwwroot");
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
