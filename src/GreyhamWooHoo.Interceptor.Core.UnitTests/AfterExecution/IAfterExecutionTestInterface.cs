using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue
{
    interface IAfterExecutionTestInterface
    {
        void TheVoidMethod();
        int TheIntMethod();
        Task TheTaskVoidMethod();
        Task<int> TheTaskIntMethod();
        Task TheExceptionTaskMethod();
    }
}
