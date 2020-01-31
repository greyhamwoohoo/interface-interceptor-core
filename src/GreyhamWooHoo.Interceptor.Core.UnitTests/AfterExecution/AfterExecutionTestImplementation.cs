using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue
{
    public class AfterExecutionTestImplementation : IAfterExecutionTestInterface
    {
        public string Message { get; set; }

        public async Task MethodReturnsAsyncVoidTask()
        {
            await Task.Run(() =>
            {
                Message = $"Invoked: {nameof(MethodReturnsAsyncVoidTask)}";
            });
        }

        public async Task<IEnumerable<Product>> MethodReturnsAsyncGenericTask()
        {
            var products = await Task<IEnumerable<Product>>.Run(() =>
            {
                Message = $"Invoked: {nameof(MethodReturnsAsyncGenericTask)}";
                
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

        public Task MethodTaskThrowsException()
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

        public Task<int> MethodReturnsTaskInt()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(250);
                Message = $"Invoked: {nameof(MethodReturnsTaskInt)}";
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

        public async void MethodIsAsyncAndReturnsVoid()
        {
            await Task.Run(() => System.Threading.Thread.Sleep(1));

            Message = $"Invoked: {nameof(MethodIsAsyncAndReturnsVoid)}";
        }
    }
}
