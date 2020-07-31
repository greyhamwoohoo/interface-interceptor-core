namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts
{
    /// <summary>
    /// Interface to test the 'OnBefore' hooks from the Interceptor. 
    /// </summary>
    /// <remarks>
    /// There is a method signature for each behavior to be tested (return values, types, Tasks, Generics and parameters)
    /// </remarks>
    public interface IBeforeExecutionMethodSignatures
    {
        void HasNoParameters();
        void HasOneParameter(int parameter1);
        void HasTwoParameters(int parameter1, int parameter2);
    }
}
