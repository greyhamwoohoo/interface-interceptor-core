using GreyhamWooHoo.Interceptor.Core.Builders;
using System;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    /// <summary>
    /// Proxy Builder Interface. 
    /// </summary>
    public interface IInterceptorProxyBuilder
    {
        object Build();
        IInterceptorProxyBuilder For(object instance);
        IInterceptorProxyBuilder InterceptAfterExecutionOf(string theMethodCalled, Action<IAfterExecutionResult> andCallbackWith);
        IInterceptorProxyBuilder InterceptAndStub(string theMethodCalled);
        IInterceptorProxyBuilder InterceptAndStub(string theMethodCalled, object withValue);
        IInterceptorProxyBuilder InterceptAndStub(string theMethodCalled, Func<IMethodCallContext, object> dynamicValueProvider);
        IInterceptorProxyBuilder InterceptBeforeExecutionOf(string theMethodNamed, Action<IBeforeExecutionResult> andCallBackWith);
        IInterceptorProxyBuilder WithTaskAwaiter(Action<Task> taskWaiter);
    }
}
