using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Microservices.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microservices.IoC
{
    public abstract class IoCAdapter : IDisposable
    {
        protected ContainerBuilder builder = new ContainerBuilder();
        protected IContainer container = null;

        protected virtual void Build()
        {
            this.container = this.builder.Build();
        }

        protected virtual IoCAdapter Register<T>(List<DIParameter> parameters = null)
        {
            this.Register(typeof(T), parameters);
            return this;
        }

        protected virtual IoCAdapter Register<T, TBase>(List<DIParameter> parameters = null)
        {
            this.Register(typeof(T), typeof(TBase), parameters);
            return this;
        }

        protected virtual IoCAdapter Register(Type type, List<DIParameter> parameters = null)
        {
            var baseType = type.GetBaseType() ?? type;
            this.Register(type, baseType, parameters);
            return this;
        }

        protected virtual IoCAdapter Register(Type type, Type baseType, List<DIParameter> parameters = null)
        {
            try
            {
                var builder = SetLifeTime(this.builder.RegisterType(type).As(baseType));

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        builder.WithParameter(
                        new ResolvedParameter(
                           (p, c) => param.Predicate(p),
                           (p, c) => param.Value
                       ));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return this;
        }

        protected virtual T Resolve<T>()
        {
            return this.container.Resolve<T>();
        }

        protected virtual object Resolve(Type type)
        {
            return this.container.Resolve(type);
        }

        protected virtual T Resolve<T>(Type type) where T : class
        {
            return this.container.Resolve(type) as T;
        }

        private delegate IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> SetLifeTimeDelegate(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder);
        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> SetPerLifeTimeScope(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder) { return builder.InstancePerLifetimeScope(); }
        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> SetSingle(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder) { return builder.SingleInstance(); }
        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> SetPerDependency(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder) { return builder.InstancePerDependency(); }
        private SetLifeTimeDelegate SetLifeTime = new SetLifeTimeDelegate(SetPerLifeTimeScope);
        public virtual IoCAdapter UsePerLifeTimeScope() { SetLifeTime = new SetLifeTimeDelegate(SetPerLifeTimeScope); return this; }
        public virtual IoCAdapter UseSingle() { SetLifeTime = new SetLifeTimeDelegate(SetSingle); return this; }
        public virtual IoCAdapter UsePerDependency() { SetLifeTime = new SetLifeTimeDelegate(SetPerDependency); return this; }


        public void Dispose()
        {
            this.container.Dispose();
            this.builder = null;
            this.container = null;
        }
    }
}
