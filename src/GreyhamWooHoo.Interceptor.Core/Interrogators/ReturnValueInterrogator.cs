using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.Interrogators
{
    public class ReturnValueInterrogator : IReturnValueInterrogator
    {
        public bool IsAsync(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            // Stack Overflow: https://stackoverflow.com/questions/20350397/how-can-i-tell-if-a-c-sharp-method-is-async-await-via-reflection
            var asyncAttribute = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>();
            return asyncAttribute != null;
        }

        public bool IsAwaitable(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            var isAwaitable = methodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
            return isAwaitable;
        }

        public bool IsTask(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            return typeof(Task).IsAssignableFrom(methodInfo.ReturnType);
        }

        public bool IsVoid(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            return methodInfo.ReturnType == typeof(void);
        }
    }
}
