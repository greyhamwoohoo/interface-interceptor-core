using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GreyhamWooHoo.Interceptor.Core.Results
{
    public class MethodCallContext : IMethodCallContext
    {
        public MethodCallContext(object[] args, IEnumerable<ParameterInfo> parameters)
        {
            Args = args ?? throw new ArgumentNullException(nameof(args));

            Parameters = new Dictionary<string, object>();

            var parameterList = parameters.ToList();
            for (int i = 0; i < parameterList.Count(); i++)
            {
                Parameters[parameterList[i].Name] = args[i];
            }
        }

        public object[] Args { get; }

        public IDictionary<string, object> Parameters { get; }
    }
}
