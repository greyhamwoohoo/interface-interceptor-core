using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept
{
    /// <summary>
    /// Interface to test all hooks in a single lifecycle: OnBefore, Stub, OnAfter
    /// </summary>
    /// <remarks>
    /// We only need a single method to test the lifecycle. 
    /// </remarks>
    public class BeforeStubAfterMethodSignatures : IBeforeStubAfterMethodSignatures
    {
        public int Echo(int value = 15)
        {
            return value;
        }
    }
}
