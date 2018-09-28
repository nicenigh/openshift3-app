using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Autofac;
using System.IO;
using Microservices.Adapter;
using Microservices.Extensions;

namespace Microservices.Adapter
{
    public class AdapterFactory : IoCAdapter
    {
        private List<IAdapter> adapters = new List<IAdapter>();
        private List<Type> adapterTypes = new List<Type>();
        private string loadPath;
        public AdapterFactory(string loadPath)
        {
            this.loadPath = loadPath;
            if (!Directory.Exists(loadPath))
                Directory.CreateDirectory(loadPath);
        }

        public AdapterFactory Build(object[] paramObjs)
        {
            var files = Directory.GetFiles(loadPath).Where(en => en.EndsWith(".dll"));
            Log.Logger.Info("Searching Adapters from: " + loadPath);

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
                    Log.Logger.Warn($"Load File Error: [{Path.GetFileName(file)}] - {e.Message}");
                }
            }

            var iAdapter = typeof(IAdapter);
            foreach (var assembly in assemblyList)
            {
                try
                {
                    var types = assembly.GetTypes().Where(en => en.IsAssignableTo<IAdapter>());
                    if (types.Count() == 0) continue;

                    adapterTypes.AddRange(types);
                }
                catch (Exception e)
                {
                    Log.Logger.Warn($"Load Assembly Error: [{assembly.FullName}] - {e.Message}");
                }
            }


            foreach (var type in adapterTypes)
            {
                try
                {
                    var paramTypes = type.GetParametersType();
                    var diParams = new Dictionary<Type, object>();
                    foreach (var pt in paramTypes)
                    {
                        var param = paramObjs.FirstOrDefault(en => en.GetType() == pt);
                        if (param != null)
                            diParams.Add(pt, param);
                        else
                            throw new Exception(string.Format("Adapter Param Type Not Found: [{0}] - {1}", type.FullName, pt.FullName));
                    }
                    base.Register(type, diParams);
                }
                catch (Exception e)
                {
                    Log.Logger.Warn($"Register Adapter Error: [{type.FullName}] - {e.Message}");
                }
            }
            base.Build();

            return this;
        }

        public AdapterFactory ResolveAll()
        {
            adapters = new List<IAdapter>();
            foreach (var type in adapterTypes)
            {
                try
                {
                    var baseType = type.GetBaseType();
                    var adapter = base.Resolve(baseType) as IAdapter;
                    adapters.Add(adapter);
                    Log.Logger.Info($"Loaded Adpater: [{type.FullName}] as [{baseType.Name}]");
                }
                catch (Exception e)
                {
                    Log.Logger.Warn($"Resolve Adapter Error: [{type.FullName}] - {e.Message}");
                }
            }
            return this;
        }

        public IAdapter GetAdapter(string adapterName)
        {
            var adapter = adapters.FirstOrDefault(en => en.GetType().GetDefineName() == adapterName);
            if (adapter != null)
                return adapter;
            var type = adapterTypes.FirstOrDefault(en => en.GetDefineName() == adapterName);
            if (type == null)
                return null;
            try
            {
                var baseType = type.GetBaseType();
                adapter = base.Resolve(baseType) as IAdapter;
                adapters.Add(adapter);
                Log.Logger.Info($"Loaded Adpater: [{type.FullName}] as [{baseType.Name}]");
            }
            catch (Exception e)
            {
                Log.Logger.Warn($"Resolve Adapter Error: [{type.FullName}] - {e.Message}");
            }
            return adapter;
        }

        public IAdapter[] GetAll()
        {
            return adapters.ToArray();
        }
    }
}
