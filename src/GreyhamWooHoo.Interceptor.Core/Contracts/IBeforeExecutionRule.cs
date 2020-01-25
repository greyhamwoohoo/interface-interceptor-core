using System;
using System.Collections.Generic;
using System.Text;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IBeforeExecutionRule : IExecutionRule
    {
        Action<IBeforeExecutionResult> Callback { get; }
        IBeforeExecutionRule Copy();
    }
}
