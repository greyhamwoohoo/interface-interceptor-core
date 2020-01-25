using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;

namespace GreyhamWooHoo.Interceptor.Core.Rules
{
    public class BeforeExecutionRule : IBeforeExecutionRule
    {
        public BeforeExecutionRule(string methodName, Action<IBeforeExecutionResult> callback)
        {
            if (null == methodName) throw new System.ArgumentNullException(nameof(methodName));
            if (null == callback) throw new System.ArgumentNullException(nameof(callback));

            MethodName = methodName;
            Callback = callback;
        }

        public string MethodName { get; }
        public Action<IBeforeExecutionResult> Callback { get; }

        public IBeforeExecutionRule Copy()
        {
            return new BeforeExecutionRule(MethodName, Callback);
        }
    }
}
