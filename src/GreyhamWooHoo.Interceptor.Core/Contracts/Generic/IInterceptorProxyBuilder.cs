using GreyhamWooHoo.Interceptor.Core.Builders;
using System;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.Contracts.Generic
{
    /// <summary>
    /// Proxy Builder Interface. 
    /// </summary>
    /// <typeparam name="T">Interface to create a proxy for. </typeparam>
    public interface IInterceptorProxyBuilder<T>
    {
        T Build();
        IInterceptorProxyBuilder<T> For(T instance);
        IInterceptorProxyBuilder<T> InterceptAfterExecutionOf(string theMethodCalled, Action<IAfterExecutionResult> andCallbackWith);
        IInterceptorProxyBuilder<T> InterceptAndStub(string theMethodCalled);
        IInterceptorProxyBuilder<T> InterceptAndStub(string theMethodCalled, object withValue);
        IInterceptorProxyBuilder<T> InterceptAndStub(string theMethodCalled, Func<IMethodCallContext, object> dynamicValueProvider);
        IInterceptorProxyBuilder<T> InterceptBeforeExecutionOf(string theMethodNamed, Action<IBeforeExecutionResult> andCallBackWith);
        IInterceptorProxyBuilder<T> WithTaskAwaiter(Action<Task> taskWaiter);
    }
}