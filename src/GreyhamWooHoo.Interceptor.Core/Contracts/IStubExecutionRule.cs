using System;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IStubExecutionRule : IExecutionRule
    {
        bool IsFixedValue { get; }
        bool IsDynamicValue { get; }
        bool IsVoid { get; }
        object Value { get; }
        Func<IMethodCallContext, object> DynamicValueProvider {get;}
    }
}
