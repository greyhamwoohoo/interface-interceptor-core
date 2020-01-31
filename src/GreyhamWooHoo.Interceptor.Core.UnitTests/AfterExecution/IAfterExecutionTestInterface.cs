using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue
{
    public interface IAfterExecutionTestInterface
    {
        void MethodReturnsVoid();
        int MethodReturnsInt();
        Task MethodReturnsTaskVoid();
        Task<int> MethodReturnsTaskInt();
        Task MethodTaskThrowsException();
        Task MethodReturnsAsyncVoidTask();
        Task<IEnumerable<Product>> MethodReturnsAsyncGenericTask();
        void MethodIsAsyncAndReturnsVoid();
    }
}
