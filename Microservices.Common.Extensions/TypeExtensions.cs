using System;
using System.Linq;

namespace Microservices.Common.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetBaseType(this Type type)
        {
            if (type.BaseType != typeof(object))
                return type.BaseType;
            else
                return type.GetInterfaces().FirstOrDefault(bt => bt != typeof(object)) ?? type.BaseType;
        }

        public static Type[] GetParametersType(this Type type)
        {
            return type.GetConstructors()?[0].GetParameters().Select(en => en.ParameterType).ToArray();
        }

        public static string GetDefineName(this Type type)
        {
            return string.Format("{0}.{1}", type.Namespace, type.Name);
        }

    }
}
