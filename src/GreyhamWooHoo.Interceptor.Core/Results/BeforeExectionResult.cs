using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GreyhamWooHoo.Interceptor.Core.Results
{
    public class BeforeExectionResult : IBeforeExecutionResult
    {
        public BeforeExectionResult(IBeforeExecutionRule rule)
        {
            Rule = rule ?? throw new System.ArgumentNullException(nameof(rule));
        }

        public BeforeExectionResult(IBeforeExecutionRule rule, object[] args, IEnumerable<ParameterInfo> parameters)
        {
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
            Args = args ?? throw new ArgumentNullException(nameof(args));

            Parameters = new Dictionary<string, object>();

            var parameterList = parameters.ToList();
            for(int i = 0; i < parameterList.Count(); i++)
            {
                Parameters[parameterList[i].Name] = args[i];
            }
        }

        public IBeforeExecutionRule Rule { get; }

        public object[] Args { get; }

        public IDictionary<string, object> Parameters { get; }
    }
}
