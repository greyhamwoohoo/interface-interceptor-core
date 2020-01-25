namespace GreyhamWooHoo.Interceptor.Core.UnitTests.LifecycleExecution
{
    public class LifecycleExecutionTestImplementation : ILifecycleExecutionTestInterface
    {
        public int Echo(int value = 15)
        {
            return value;
        }
    }
}
