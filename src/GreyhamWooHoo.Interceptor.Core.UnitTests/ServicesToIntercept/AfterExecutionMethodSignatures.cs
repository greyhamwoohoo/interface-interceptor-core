using GreyhamWooHoo.Interceptor.Core.UnitTests.Exceptions;
using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept
{
    /// <summary>
    /// Methods to test the 'OnAfter' hooks from the Interceptor. 
    /// </summary>
    /// <remarks>
    /// There is a method signature for each behavior to be tested (return values, types, Tasks, Generics and parameters)
    /// </remarks>
    public class AfterExecutionMethodSignatures : IAfterExecutionMethodSignatures
    {
        public string Message { get; set; }

        public void IsVoid()
        {
            Message = $"Invoked: {nameof(IsVoid)}";
        }

        public int HasNoParameters()
        {
            return 0;
        }

        public int HasOneParameter(int param1)
        {
            return 1;
        }

        public int HasTwoParameters(string param1, int param2)
        {
            return 2;
        }

        public int ReturnsIntWithValue10()
        {
            Message = $"Invoked: {nameof(ReturnsIntWithValue10)}";
            return 10;
        }

        public Task ReturnsTaskThatIsVoid()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(250);
                Message = $"Invoked: {nameof(ReturnsTaskThatIsVoid)}";
            });
        }

        public Task ReturnsTaskResultThatIsAnIntWithValue25()
        {
            return Task.Run(() =>
            {
                Message = $"Invoked: {nameof(ReturnsTaskResultThatIsAnIntWithValue25)}";
                return 25;
            });
        }

        public Task<int> ReturnsGenericTaskResultThatIsAnIntWithValue10()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(250);
                Message = $"Invoked: {nameof(ReturnsGenericTaskResultThatIsAnIntWithValue10)}";
                return 10;
            });
        }

        public Task ReturnsTaskButThrowsAnExceptionInstead()
        {
            return Task.Run(() =>
            {
                throw new CustomTaskException();
            });
        }

        public async void AsyncIsVoid()
        {
            await Task.Run(() => System.Threading.Thread.Sleep(1));

            Message = $"Invoked: {nameof(AsyncIsVoid)}";
        }

        public async Task AsyncReturnsVoidTask()
        {
            await Task.Run(() =>
            {
                Message = $"Invoked: {nameof(AsyncReturnsVoidTask)}";
            });
        }

        public async Task<IEnumerable<Product>> AsyncReturnsGenericTaskResult()
        {
            var products = await Task.Run(() =>
            {
                Message = $"Invoked: {nameof(AsyncReturnsGenericTaskResult)}";

                var result = new Product[2]
                {
                    new Product()
                    {
                        Name = "Name1",
                        Description = "Description1"
                    },
                    new Product()
                    {
                        Name = "Name2",
                        Description = "Description2"
                    }
                };

                return result;
            });

            return products;
        }
    }
}
