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
        Task MethodReturnsTaskIntResult();
        Task<int> MethodReturnsGenericTaskInt();
        Task MethodReturnsTaskButThrowsException();
        void MethodReturnsVoidAsync();
        Task MethodReturnsTaskVoidAsync();
        Task<IEnumerable<Product>> MethodReturnsGenericTaskAsync();
        int MethodAsNoParameters();
        int MethodHasOneParameter(int theInt);
        int MethodHasTwoParameters(string theString, int theInt);
    }
}
