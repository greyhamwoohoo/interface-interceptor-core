using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue
{
    public class AfterExecutionTestImplementation : IAfterExecutionTestInterface
    {
        public string Message { get; set; }

        public async Task MethodReturnsTaskVoidAsync()
        {
            await Task.Run(() =>
            {
                Message = $"Invoked: {nameof(MethodReturnsTaskVoidAsync)}";
            });
        }

        public async Task<IEnumerable<Product>> MethodReturnsGenericTaskAsync()
        {
            var products = await Task<IEnumerable<Product>>.Run(() =>
            {
                Message = $"Invoked: {nameof(MethodReturnsGenericTaskAsync)}";

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

        public Task MethodReturnsTaskButThrowsException()
        {
            return Task.Run(() =>
            {
                throw new CustomTaskException();
            });
        }

        public int MethodReturnsInt()
        {
            Message = $"Invoked: {nameof(MethodReturnsInt)}";
            return 10;
        }

        public Task<int> MethodReturnsGenericTaskInt()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(250);
                Message = $"Invoked: {nameof(MethodReturnsGenericTaskInt)}";
                return 10;
            });
        }

        public Task MethodReturnsTaskVoid()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(250);
                Message = $"Invoked: {nameof(MethodReturnsTaskVoid)}";
            });
        }

        public void MethodReturnsVoid()
        {
            Message = $"Invoked: {nameof(MethodReturnsVoid)}";
        }

        public Task MethodReturnsTaskIntResult()
        {
            return Task.Run(() =>
            {
                Message = $"Invoked: {nameof(MethodReturnsTaskIntResult)}";
                return 25;
            });
        }

        public async void MethodReturnsVoidAsync()
        {
            await Task.Run(() => System.Threading.Thread.Sleep(1));

            Message = $"Invoked: {nameof(MethodReturnsVoidAsync)}";
        }

        public void MethodAsNoParameters()
        {
        }

        public void MethodHasOneParameter(int param1)
        {
        }

        public void MethodHasTwoParameters(string param1, int param2)
        {
        }
    }
}
