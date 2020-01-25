using System.Collections.Generic;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IBeforeExecutionResult
    {
        IBeforeExecutionRule Rule { get; }

        object[] Args { get; }
        IDictionary<string, object> Parameters { get; }
    }
}
