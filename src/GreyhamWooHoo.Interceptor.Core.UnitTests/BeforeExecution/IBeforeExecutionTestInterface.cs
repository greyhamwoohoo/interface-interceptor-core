namespace GreyhamWooHoo.Interceptor.Core.UnitTests.BeforeExecution
{
    public interface IBeforeExecutionTestInterface
    {
        void MethodWithNoParameters();
        void MethodWithOneParameter(int parameter1);
        void MethodWithTwoParameters(int parameter1, int parameter2);
    }
}
