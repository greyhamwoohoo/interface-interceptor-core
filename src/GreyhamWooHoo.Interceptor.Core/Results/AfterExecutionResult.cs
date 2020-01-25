using GreyhamWooHoo.Interceptor.Core.Contracts;

namespace GreyhamWooHoo.Interceptor.Core.Results
{
    public class AfterExecutionResult : IAfterExecutionResult
    {
        public AfterExecutionResult(IAfterExecutionRule rule) : this(rule, false, null)
        {
        }

        public AfterExecutionResult(IAfterExecutionRule rule, bool hasReturnValue, object returnValue)
        {
            Rule = rule;
            HasReturnValue = hasReturnValue;
            ReturnValue = returnValue;
        }

        public IAfterExecutionRule Rule { get; }

        public bool HasReturnValue { get; }

        public object ReturnValue { get; }
    }
}
