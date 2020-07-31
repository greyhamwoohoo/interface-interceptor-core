using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts
{
    /// <summary>
    /// Interface to test the 'OnAfter' hooks from the Interceptor. 
    /// </summary>
    /// <remarks>
    /// There is a method signature for each behavior to be tested (return values, types, Tasks, Generics and parameters)
    /// </remarks>
    public interface IAfterExecutionMethodSignatures
    {
        void IsVoid();
        int HasNoParameters();
        int HasOneParameter(int theInt);
        int HasTwoParameters(string theString, int theInt);
        int ReturnsIntWithValue10();
        Task ReturnsTaskThatIsVoid();
        Task ReturnsTaskResultThatIsAnIntWithValue25();
        Task<int> ReturnsGenericTaskResultThatIsAnIntWithValue10();
        Task ReturnsTaskButThrowsAnExceptionInstead();
        void AsyncIsVoid();
        Task AsyncReturnsVoidTask();
        Task<IEnumerable<Product>> AsyncReturnsGenericTaskResult();
    }
}
