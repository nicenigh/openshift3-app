using Autofac;
using Microservices.Base;
using Microservices.Common;
using Microservices.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.IoC
{
    public class AdapterFac
    {
        public static readonly AdapterFac Instance = new AdapterFac();

        private AdapterFac() { }

        private readonly Dictionary<Type, IAdapter> Adapters = new Dictionary<Type, IAdapter>();

        public AdapterFac Register(Type adapterType, Config config = null)
        {
            if (adapterType.IsAssignableTo<IAdapter>())
            {
                try
                {
                    var paramTypes = adapterType.GetParametersType();
                    var diParams = new List<object>();
                    foreach (var pt in paramTypes)
                    {
                        if (pt.IsAssignableTo<Config>())
                            diParams.Add(config);
                    }
                    var adapter = Activator.CreateInstance(adapterType, diParams.ToArray()) as IAdapter;
                    Adapters.Add(adapterType, adapter);
                }
                catch (Exception e)
                {
                    Log.Logger.Warn($"Register Adapter Error: [{adapterType.FullName}]", e);
                }
            }
            return this;
        }

        public IAdapter GetAdapter(Type adapterType)
        {
            return Adapters.FirstOrDefault(en => adapterType.IsAssignableFrom(en.Key)).Value;
        }

        public IAdapter GetAdapter<TAdapter>()
        {
            return Adapters.FirstOrDefault(en => (typeof(TAdapter).IsAssignableFrom(en.Key))).Value;
        }

        public IAdapter[] GetAdapters()
        {
            return Adapters.Values.ToArray();
        }
    }
}
