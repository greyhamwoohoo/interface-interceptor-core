using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.Interrogators
{
    public class MethodInterrogator : IMethodInterrogator
    {
        public bool IsAsync(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            // Stack Overflow: https://stackoverflow.com/questions/20350397/how-can-i-tell-if-a-c-sharp-method-is-async-await-via-reflection
            var asyncAttribute = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>();
            return asyncAttribute != null;
        }
        public bool IsVoid(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            return methodInfo.ReturnType == typeof(void);
        }

        public bool IsAwaitable(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            var isAwaitable = ReturnsTask(methodInfo);
            return isAwaitable;
        }

        public bool ReturnsGenericTask(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            var returnType = methodInfo.ReturnType;
            var isGenericTask= returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>);
            return isGenericTask;
        }

        public bool ReturnsTask(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            return typeof(Task).IsAssignableFrom(methodInfo.ReturnType);
        }
    }
}
