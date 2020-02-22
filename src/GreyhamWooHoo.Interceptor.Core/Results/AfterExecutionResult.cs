using GreyhamWooHoo.Interceptor.Core.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GreyhamWooHoo.Interceptor.Core.Results
{
    public class AfterExecutionResult : IAfterExecutionResult
    {
        public AfterExecutionResult(IAfterExecutionRule rule, object[] args, IEnumerable<ParameterInfo> parameters) : this(rule, false, null, args, parameters)
        {
        }

        public AfterExecutionResult(IAfterExecutionRule rule, bool hasReturnValue, object returnValue, object[] args, IEnumerable<ParameterInfo> parameters)
        {
            Rule = rule;
            HasReturnValue = hasReturnValue;
            ReturnValue = returnValue;
            Args = args;

            Parameters = new Dictionary<string, object>();

            var parameterList = parameters.ToList();
            for (int i = 0; i < parameterList.Count(); i++)
            {
                Parameters[parameterList[i].Name] = args[i];
            }
        }

        public IAfterExecutionRule Rule { get; }

        public bool HasReturnValue { get; }

        public object ReturnValue { get; }

        public object[] Args { get; }

        public IDictionary<string, object> Parameters { get; }
    }
}
