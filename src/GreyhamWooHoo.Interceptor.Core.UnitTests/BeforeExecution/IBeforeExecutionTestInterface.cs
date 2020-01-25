namespace GreyhamWooHoo.Interceptor.Core.UnitTests.BeforeExecution
{
    public interface IBeforeExecutionTestInterface
    {
        void TheMethodWithNoParameters();
        void TheMethodWithOneParameter(int parameter1);
        void TheMethodWithManyParameters(int parameter1, int parameter2);
    }
}
