namespace GreyhamWooHoo.Interceptor.Core.UnitTests.BeforeExecution
{
    public class BeforeExecutionTestImplementation : IBeforeExecutionTestInterface
    {
        public string Message { get; set; }

        public void TheMethodWithManyParameters(int parameter1, int parameter2)
        {
            Message = $"Invoked: {parameter1} {parameter2}";
        }

        public void TheMethodWithNoParameters()
        {
            Message = "Invoked";
        }

        public void TheMethodWithOneParameter(int parameter1)
        {
            Message = $"Invoked: {parameter1}";
        }
    }
}
