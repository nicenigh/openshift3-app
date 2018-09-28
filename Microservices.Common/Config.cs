using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Microservices.Common
{
    public class Config
    {
        public static readonly Config Root = new Config();
        public static Config New { get { return new Config(); } }
        private IConfiguration config = null;
        private Config() { }
        private Config(IConfiguration config) { this.config = config; }
        public IConfiguration GetConfig() { return config; }
        public Config Load(params string[] files)
        {
            if (config != null) throw new Exception();
            var builder = new ConfigurationBuilder();
            foreach (var file in files)
            {
                if (file.EndsWith(".xml"))
                    builder.AddXmlFile(file, optional: true, reloadOnChange: true);
                else if (file.EndsWith(".json"))
                    builder.AddJsonFile(file, optional: true, reloadOnChange: true);
            }
            config = builder.Build();
            return this;
        }
        public Config Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return this;
            else
                return new Config(config.GetChildren().FirstOrDefault(en => en.Key == key));
        }
        public Config this[string key] { get { return Get(key); } }
        public override string ToString() { return ((IConfigurationSection)config).Value; }
        public static implicit operator String(Config config) { return config.ToString(); }
    }
}

