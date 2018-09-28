using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microservices.IoC
{
    public class DIParameter
    {
        public DIParameter(Func<ParameterInfo, bool> predicate, object value)
        {
            this.Predicate = predicate ?? throw new Exception();
            this.Value = value;
        }
        public Func<ParameterInfo, bool> Predicate { get; }
        public object Value { get; }
    }
}
