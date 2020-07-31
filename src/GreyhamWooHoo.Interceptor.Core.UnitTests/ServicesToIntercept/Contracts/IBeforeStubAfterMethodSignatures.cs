namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts
{
    /// <summary>
    /// Interface to test all hooks in a single lifecycle: OnBefore, Stub, OnAfter
    /// </summary>
    /// <remarks>
    /// We only need a single method to test the lifecycle. 
    /// </remarks>
    public interface IBeforeStubAfterMethodSignatures
    {
        int Echo(int value);
    }
}
