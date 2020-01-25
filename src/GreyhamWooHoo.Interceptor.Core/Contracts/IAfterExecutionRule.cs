using System;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IAfterExecutionRule : IExecutionRule
    {
        Action<IAfterExecutionResult> Callback { get; }

        IAfterExecutionRule Copy();
    }
}
