using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;

namespace GreyhamWooHoo.Interceptor.Core.Rules
{
    public class StubExecutionRule : IStubExecutionRule
    {
        public StubExecutionRule(string methodName, object value)
        {
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            Value = value;
        }
        public object Value { get; }

        public string MethodName { get; }
    }
}
