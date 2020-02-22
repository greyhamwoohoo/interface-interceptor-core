using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    /// <summary>
    /// Interrogates a 
    /// </summary>
    public interface IMethodInterrogator
    {
        bool IsVoid(MethodInfo methodInfo);
        bool ReturnsTask(MethodInfo methodInfo);
        bool IsAsync(MethodInfo methodInfo);
        bool IsAwaitable(MethodInfo methodInfo);
        bool ReturnsGenericTask(MethodInfo methodInfo);
    }
}
