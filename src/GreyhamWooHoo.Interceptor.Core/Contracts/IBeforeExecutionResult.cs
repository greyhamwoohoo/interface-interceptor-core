using System.Collections.Generic;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IBeforeExecutionResult : IMethodCallContext
    {
        IBeforeExecutionRule Rule { get; }
    }
}
