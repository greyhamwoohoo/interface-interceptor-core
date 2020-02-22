using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Interrogators;
using GreyhamWooHoo.Interceptor.Core.Results;
using GreyhamWooHoo.Interceptor.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core
{
    /// <summary>
    /// The interceptor will apply rules built using the InterceptorProxyBuilder. Depending on the rules specified, the interceptor will either callout BeforeExecution, AfterExecution or Stub the method. 
    /// </summary>
    /// <remarks>
    /// While the following reference is for Aspect Oriented Programming (static/source-code-level attributes), I have used the general pattern for this interceptor solution:
    /// Reference: https://www.c-sharpcorner.com/article/aspect-oriented-programming-in-c-sharp-using-dispatchproxy/
    /// </remarks>
    /// <typeparam name="T">Interface to be intercepted. </typeparam>
    public class InterceptorProxy<T> : DispatchProxy
    {
        private T _originalImplementation;
        private IEnumerable<IBeforeExecutionRule> _beforeExecutionRules;
        private IEnumerable<IStubExecutionRule> _stubExecutionRules;
        private IEnumerable<IAfterExecutionRule> _afterExecutionRules;
        private Action<Task> _taskWaiter;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;

            ExecuteBeforeExecutionRules(forTargetMethod: targetMethod, withArgs: args);

            var result = ExecuteStubExecutionRules(forTargetMethod: targetMethod, withArgs: args);

            if (_afterExecutionRules.Count() == 0)
            {
                return result;
            }

            var invocationResult = default(InvocationResult);

            if (result is Task task)
            {
                invocationResult = WaitForResultOf(task, targetMethod);
            }
            else
            {
                // We now callout with the original value: it is better the recipient serialize / deserialize this by casting to the expected type first. 
                if (targetMethod.ReturnType == typeof(void))
                {
                    invocationResult = new InvocationResult();
                }
                else
                {
                    invocationResult = new InvocationResult()
                    {
                        HasReturnValue = true,
                        ReturnValue = result
                    };
                }
            }

            _afterExecutionRules.ToList().ForEach(ar =>
            {
                try
                {
                    ar.Callback(new AfterExecutionResult(ar, invocationResult.HasReturnValue, invocationResult.ReturnValue, args, targetMethod.GetParameters()));
                }
                catch (Exception)
                {
                    // Design decision: if anything goes wrong in the callback, we do not want to change the result of invoking the method. Therefore, sink the exception. 
                }
            });

            return result;
        }

        private InvocationResult WaitForResultOf(Task task, MethodInfo targetMethod)
        {
            var methodInterrogator = new MethodInterrogator();
            var invocationResult = default(InvocationResult);

            // Callback to wait for the task to finish. For some long running tasks, there is the chance a task would not complete before the test finishes... 
            _taskWaiter(task);

            object taskResult = null;

            if (methodInterrogator.ReturnsGenericTask(targetMethod))
            {
                // SCENARIO:
                // A method that returns a Generic Task will *ALWAYS* have a result. 

                // ie:
                // Task<int> TheMethod()
                // async Task<T> TheMethod()

                // Reference:
                // https://www.c-sharpcorner.com/article/aspect-oriented-programming-in-c-sharp-using-dispatchproxy/");
                var property = task.GetType().GetTypeInfo().GetProperties().FirstOrDefault(p => p.Name == "Result");
                if (property != null)
                {
                    taskResult = property.GetValue(task);
                }

                invocationResult = new InvocationResult()
                {
                    HasReturnValue = true,
                    ReturnValue = taskResult
                };
            }
            else
            {
                var property = task.GetType().GetTypeInfo().GetProperties().FirstOrDefault(p => p.Name == "Result");
                if(property != null)
                {
                    taskResult = property.GetValue(task);

                    // Looks like the only way to test for a 'Void' Task. https://stackoverflow.com/questions/59080219/get-the-result-of-funcobject-when-object-is-a-tasksomething
                    if (taskResult.GetType().FullName == "System.Threading.Tasks.VoidTaskResult")
                    {
                        // SCENARIO:
                        // Async Task TheMethod() { return Task.Run(() => {  });
                        invocationResult = new InvocationResult();
                    }
                    else
                    {
                        // SCENARIO:
                        // Task TheMethod() { return Task.Run(() => { return something; });
                        invocationResult = new InvocationResult()
                        {
                            HasReturnValue = true,
                            ReturnValue = taskResult
                        };
                    }
                }
                else
                {
                    // SCENARIO:
                    // Task TheMethod() { return Task.Run(() => { returns nothing });
                    invocationResult = new InvocationResult();
                }
            }

            return invocationResult;
        }

        private void ExecuteBeforeExecutionRules(MethodInfo forTargetMethod, object[] withArgs)
        {
            var beforeRules = _beforeExecutionRules.Where(f => f.MethodName == forTargetMethod.Name);
            beforeRules.ToList().ForEach(beforeRule =>
            {
                try
                {
                    var parameters = forTargetMethod.GetParameters();
                    var beforeExecutionResult = new BeforeExectionResult(beforeRule, withArgs, parameters);
                    beforeRule.Callback(beforeExecutionResult);
                }
                catch
                {
                    // Design Decision: if something goes wrong in the callout, we sink it and continue execution. 
                }
            });
        }

        private object ExecuteStubExecutionRules(MethodInfo forTargetMethod, object[] withArgs)
        {
            // If stubbed: use that value for the result and continue; else invoke the original method. 
            var stubbedRule = _stubExecutionRules.FirstOrDefault(f => f.MethodName == forTargetMethod.Name);
            var result = default(object);
            if (stubbedRule != null)
            {
                result = stubbedRule.Value;
            }
            else
            {
                result = forTargetMethod.Invoke(_originalImplementation, withArgs);
            }

            return result;
        }

        private void SetParameters(T originalImplementation, IEnumerable<IBeforeExecutionRule> beforeExecutionRules, IEnumerable<IStubExecutionRule> stubExecutionRules, IEnumerable<IAfterExecutionRule> afterExecutionRules, Action<Task> taskWaiter)
        {
            _originalImplementation = originalImplementation;
            _taskWaiter = taskWaiter;

            _beforeExecutionRules = beforeExecutionRules.Select(i => new BeforeExecutionRule(i.MethodName, i.Callback));
            _afterExecutionRules = afterExecutionRules.Select(i => new AfterExecutionRule(i.MethodName, i.Callback));
            _stubExecutionRules = stubExecutionRules.Select(i => new StubExecutionRule(i.MethodName, i.Value));
        }

        public static T Create(T originalImplementation, IEnumerable<IBeforeExecutionRule> beforeExecutionRules, IEnumerable<IStubExecutionRule> stubExecutionRules, IEnumerable<IAfterExecutionRule> afterExecutionRules, Action<Task> taskWaiter)
        {
            if (null == originalImplementation) throw new ArgumentNullException(nameof(originalImplementation));
            if (null == beforeExecutionRules) throw new ArgumentNullException(nameof(beforeExecutionRules));
            if (null == stubExecutionRules) throw new ArgumentNullException(nameof(stubExecutionRules));
            if (null == afterExecutionRules) throw new ArgumentNullException(nameof(afterExecutionRules));
            if (null == taskWaiter) throw new ArgumentNullException(nameof(taskWaiter));

            object proxy = Create<T, InterceptorProxy<T>>();

            ((InterceptorProxy<T>)proxy).SetParameters(originalImplementation, beforeExecutionRules, stubExecutionRules, afterExecutionRules, taskWaiter);

            return (T)proxy;
        }

        internal class InvocationResult
        {

            internal bool HasReturnValue { get; set; }
            internal object ReturnValue { get; set; }
        }
    }
}
