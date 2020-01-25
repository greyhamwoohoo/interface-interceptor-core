using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.Builders
{
    /// <summary>
    /// Build Interception rules for an interface.  
    /// </summary>
    /// <remarks>
    /// Interception rules include OnBeforeExecution, OnAfterExecution and Stubbing. 
    /// </remarks>
    /// <typeparam name="T">The interface whose methods are to be intercepted.</typeparam>
    public class InterceptorProxyBuilder<T>
    {
        private T _instance;
        private Action<Task> _taskWaiter;

        private readonly List<IBeforeExecutionRule> BeforeExecutionRules;
        private readonly List<IAfterExecutionRule> AfterExecutionRules;
        private readonly List<IStubExecutionRule> StubExecutionRules;

        public InterceptorProxyBuilder()
        {
            BeforeExecutionRules = new List<IBeforeExecutionRule>();
            AfterExecutionRules = new List<IAfterExecutionRule>();
            StubExecutionRules = new List<IStubExecutionRule>();

            _taskWaiter = task => task.Wait();
        }

        public InterceptorProxyBuilder<T> For(T instance)
        {
            _instance = instance;
            return this;
        }

        /// <summary>
        /// By default, we will .Wait() on any Tasks that are returned from an intercepted method. This is to ensure the value is available to the caller immediately. 
        /// </summary>
        /// <param name="taskWaiter">A callback with the task to wait on (or not). </param>
        /// <returns></returns>
        public InterceptorProxyBuilder<T> WithTaskAwaiter(Action<Task> taskWaiter)
        {
            _taskWaiter = taskWaiter;
            return this;
        }

        /// <summary>
        /// Callback after the method has been executed. The return value of the method is passed to the callback. In the case of an error, the callback will not be invoked. 
        /// </summary>
        /// <param name="theMethodCalled">Method name to intercept. </param>
        /// <param name="andCallbackWith">Callback to invoke after method has executed. </param>
        /// <returns></returns>
        public InterceptorProxyBuilder<T> InterceptAfterExecutionOf(string theMethodCalled, Action<IAfterExecutionResult> andCallbackWith)
        {
            AfterExecutionRules.Add(new AfterExecutionRule(theMethodCalled, andCallbackWith));
            return this;
        }

        /// <summary>
        /// Intercept a void method; in essence, making it do nothing. 
        /// </summary>
        /// <param name="theMethodCalled">Method name to intercept. </param>
        /// <returns></returns>
        public InterceptorProxyBuilder<T> InterceptAndStub(string theMethodCalled)
        {
            StubExecutionRules.Add(new StubExecutionRule(theMethodCalled, null));
            return this;
        }

        /// <summary>
        /// Intercept a method and return the given value. 
        /// </summary>
        /// <param name="theMethodCalled">Method name to stub.</param>
        /// <param name="withValue">Value to return from stubbed method. </param>
        /// <returns></returns>
        public InterceptorProxyBuilder<T> InterceptAndStub(string theMethodCalled, object withValue)
        {
            StubExecutionRules.Add(new StubExecutionRule(theMethodCalled, withValue));
            return this;
        }

        /// <summary>
        /// Intercept a method before it is executed. 
        /// </summary>
        /// <param name="theMethodNamed">Method name to intercept. </param>
        /// <param name="andCallBackWith">Callback to invoke before execution. The callback is passed the args, method name, rule and parameter dictionary. </param>
        /// <returns></returns>
        public InterceptorProxyBuilder<T> InterceptBeforeExecutionOf(string theMethodNamed, Action<IBeforeExecutionResult> andCallBackWith)
        {
            BeforeExecutionRules.Add(new BeforeExecutionRule(theMethodNamed, andCallBackWith));
            return this;
        }

        /// <summary>
        /// Build the interceptor. 
        /// </summary>
        /// <returns></returns>
        public T Build()
        {
            if (null == _instance) throw new InvalidOperationException($"You must call the {nameof(For)} method and pass in a concrete instance of the interface implementation. ");

            return InterceptorProxy<T>.Create(_instance, BeforeExecutionRules, StubExecutionRules, AfterExecutionRules, _taskWaiter);
        }
    }
}
