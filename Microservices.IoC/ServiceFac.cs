using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Autofac;
using System.IO;
using Microservices.Common.Extensions;
using Microservices.Common;
using Microservices.Base;

namespace Microservices.IoC
{
    public class ServiceFac
    {
        public static readonly ServiceFac Instance = new ServiceFac();
        private readonly Dictionary<string, Type> Services = new Dictionary<string, Type>();

        private ServiceFac() { }

        public ServiceFac Load(string loadPath)
        {
            if (!Directory.Exists(loadPath))
                Directory.CreateDirectory(loadPath);
            var files = Directory.GetFiles(loadPath).Where(en => en.EndsWith(".dll"));
            Log.Logger.Info("Searching Servies from: " + loadPath);
            var assemblyList = new List<Assembly>();

            foreach (var file in files)
            {
                try
                {
                    var path = Path.Combine(loadPath, Path.GetFileName(file));
                    Assembly assembly = Assembly.Load(File.ReadAllBytes(path));
                    if (assembly == null) continue;
                    assemblyList.Add(assembly);
                }
                catch (Exception e)
                {
                    Log.Logger.Warn($"Load File Error: [{Path.GetFileName(file)}]", e);
                }
            }

            var serviceList = new List<IDefinition>();


            foreach (var assembly in assemblyList)
            {
                try
                {
                    var types = assembly.GetTypes().Where(en => en.IsAssignableTo<IDefinition>());
                    if (types.Count() == 0) continue;
                    var name = assembly.GetName().Name;
                    Log.Logger.Info($"Loading Assembly: [{name}]");

                    foreach (var t in types)
                    {
                        if (!Services.ContainsKey(t.Name))
                            Services.Add(t.Name, t);
                    }
                }
                catch (Exception e)
                {
                    Log.Logger.Warn($"Load Assembly Error: [{assembly.FullName}]", e);
                }
            }
            return this;
        }

        public Type GetType(string typeName)
        {
            if (Services.ContainsKey(typeName))
                return Services[typeName];
            else
                return null;
        }

        public Type[] GetTypes(Type baseType)
        {
            return Services.Values.Where(en => baseType.IsAssignableFrom(baseType)).ToArray();
        }

        public Type[] GetTypes<TBaseType>()
        {
            return Services.Values.Where(en => (typeof(TBaseType).IsAssignableFrom(en))).ToArray();
        }

        public Type[] GetTypes(string defineName)
        {
            return Services.Values.Where(en => en.GetDefineName() == defineName).ToArray();
        }

        public Type[] GetTypes(string nameSpace, string defineName)
        {
            return Services.Values.Where(en => en.Assembly.GetName().Name == nameSpace && en.GetDefineName() == defineName).ToArray();
        }

    }
}
