using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue
{
    public class AfterExecutionTestImplementation : IAfterExecutionTestInterface
    {
        public string Message { get; set; }

        public Task TheExceptionTaskMethod()
        {
            return Task.Run(() =>
            {
                throw new CustomTaskException();
            });
        }

        public int TheIntMethod()
        {
            Message = $"Invoked: {nameof(TheIntMethod)}";
            return 10;
        }

        public Task<int> TheTaskIntMethod()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(250);
                Message = $"Invoked: {nameof(TheTaskIntMethod)}";
                return 10;
            });
        }

        public Task TheTaskVoidMethod()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(250);
                Message = $"Invoked: {nameof(TheTaskVoidMethod)}";
            });
        }

        public void TheVoidMethod()
        {
            Message = $"Invoked: {nameof(TheVoidMethod)}";
        }
    }
}
