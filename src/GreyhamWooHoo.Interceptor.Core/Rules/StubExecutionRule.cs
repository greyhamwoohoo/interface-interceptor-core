using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;

namespace GreyhamWooHoo.Interceptor.Core.Rules
{
    public class StubExecutionRule : IStubExecutionRule
    {
        public StubExecutionRule(string methodName)
        {
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            IsFixedValue = false;
        }

        public StubExecutionRule(string methodName, object value) : this(methodName, value, true)
        {
        }
        public StubExecutionRule(string methodName, Func<IMethodCallContext, object> withValueProvider)
        {
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            DynamicValueProvider = withValueProvider ?? throw new ArgumentNullException(nameof(withValueProvider));

            IsFixedValue = false;
        }

        public StubExecutionRule(string methodName, object value, bool isFixedValue)
        {
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            Value = value;
            IsFixedValue = isFixedValue;
        }
        public object Value { get; }

        public string MethodName { get; }

        public bool IsFixedValue { get; }
        public bool IsDynamicValue => DynamicValueProvider != null;
        public bool IsVoid => !IsFixedValue && !IsDynamicValue;

        public Func<IMethodCallContext, object> DynamicValueProvider { get; }
    }
}
