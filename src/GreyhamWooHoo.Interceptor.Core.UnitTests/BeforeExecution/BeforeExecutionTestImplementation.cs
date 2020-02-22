namespace GreyhamWooHoo.Interceptor.Core.UnitTests.BeforeExecution
{
    public class BeforeExecutionTestImplementation : IBeforeExecutionTestInterface
    {
        public string Message { get; set; }

        public void MethodWithNoParameters()
        {
            Message = "Invoked";
        }
        public void MethodWithOneParameter(int parameter1)
        {
            Message = $"Invoked: {parameter1}";
        }
        public void MethodWithTwoParameters(int parameter1, int parameter2)
        {
            Message = $"Invoked: {parameter1} {parameter2}";
        }


    }
}
