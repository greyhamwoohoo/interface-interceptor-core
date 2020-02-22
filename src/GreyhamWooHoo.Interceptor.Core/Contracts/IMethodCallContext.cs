using System.Collections.Generic;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IMethodCallContext
    {
        object[] Args { get; }
        IDictionary<string, object> Parameters { get; }
    }
}
