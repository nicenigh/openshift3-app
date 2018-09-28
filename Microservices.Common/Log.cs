using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Common
{
    public class Log
    {
        public static readonly Log Logger = new Log();
        private ILoggerRepository repository = LogManager.CreateRepository("Moons");
        private Log()
        {
            XmlConfigurator.Configure(repository, new FileInfo(Path.Combine(Environment.CurrentDirectory, "log4net.config")));
        }
        private ILog logger
        {
            get
            {
                return LogManager.GetLogger(repository.Name, Thread.CurrentThread.Name ?? "unnamed");
            }
        }

        public static void SetName(string name)
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = name;
        }

        public void Info(string format, params object[] args) { Task.Run(() => { logger.InfoFormat(format, args); }); }
        public void Info(string message, Exception exception) { Task.Run(() => { logger.Info(message, exception); }); }
        public void Warn(string format, params object[] args) { Task.Run(() => { logger.WarnFormat(format, args); }); }
        public void Warn(string message, Exception exception) { Task.Run(() => { logger.Warn(message, exception); }); }
        public void Error(string format, params object[] args) { Task.Run(() => { logger.ErrorFormat(format, args); }); }
        public void Error(string message, Exception exception) { Task.Run(() => { logger.Error(message, exception); }); }
        public void Fatal(string format, params object[] args) { Task.Run(() => { logger.FatalFormat(format, args); }); }
        public void Fatal(string message, Exception exception) { Task.Run(() => { logger.Fatal(message, exception); }); }
    }
}
