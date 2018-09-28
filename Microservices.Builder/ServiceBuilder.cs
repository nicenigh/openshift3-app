using Microservices.Common;
using Microservices.IoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microservices.Builder
{
    public class ServiceBuilder
    {
        private List<Action<ServiceBuilder>> configure = new List<Action<ServiceBuilder>>();
        public ServiceBuilder ConfigureServices(Action<ServiceBuilder> configureServices)
        {
            configure.Add(configureServices);
            return this;
        }
        public void Run()
        {
            System.Threading.Thread.CurrentThread.Name = "main";
            var logger = Log.Logger;

            ServiceFac.Instance.Load(Path.Combine(Environment.CurrentDirectory, "services"));

            var tasks = new List<Task>();
            foreach (var action in configure)
            {
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        action.Invoke(this);
                    }
                    catch (Exception e)
                    {
                        logger.Error("Server Error", e);
                    }
                }));
            }
            logger.Info("Server Start");
            Task.WaitAll(tasks.ToArray());
        }
    }
}