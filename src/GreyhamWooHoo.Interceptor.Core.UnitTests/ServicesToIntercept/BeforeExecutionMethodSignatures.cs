using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept
{
    /// <summary>
    /// Methods to test the 'OnBefore' hooks from the Interceptor. 
    /// </summary>
    /// <remarks>
    /// There is a method signature for each behavior to be tested (return values, types, Tasks, Generics and parameters)
    /// </remarks>
    public class BeforeExecutionMethodSignatures : IBeforeExecutionMethodSignatures
    {
        public string Message { get; set; }

        public void HasNoParameters()
        {
            Message = "Invoked";
        }
        public void HasOneParameter(int parameter1)
        {
            Message = $"Invoked: {parameter1}";
        }
        public void HasTwoParameters(int parameter1, int parameter2)
        {
            Message = $"Invoked: {parameter1} {parameter2}";
        }


    }
}
