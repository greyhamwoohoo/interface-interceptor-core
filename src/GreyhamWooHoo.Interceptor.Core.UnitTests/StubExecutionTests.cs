using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class StubExecutionTests : AfterTestBase
    {
        [TestMethod]
        public void MethodIsVoid()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsVoid))
                .Build();

            // Act
            proxy.MethodReturnsVoid();

            // Assert
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
        }

        [TestMethod]
        public void MethodIsVoidNotIntercepted()
        {
            // Arrange, Act
            _originalImplementation.MethodReturnsVoid();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsVoid)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public void MethodReturnsInt()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsInt), withValue: 15)
                .Build();

            // Act
            var result = proxy.MethodReturnsInt();

            // Assert
            result.Should().Be(15, because: "that is the stubbed value");
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
        }

        [TestMethod]
        public void MethodReturnsIntNotIntercepted()
        {
            // Arrange, Act
            _originalImplementation.MethodReturnsInt();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsInt)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public async Task MethodReturnsTaskVoid()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoid), Task.CompletedTask)
                .Build();

            // Act
            await proxy.MethodReturnsTaskVoid();

            // Assert
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
        }

        [TestMethod]
        public async Task MethodReturnsTaskVoidNotIntercepted()
        {
            // Arrange, Act
            await _originalImplementation.MethodReturnsTaskVoid();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoid)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public async Task MethodReturnsGenericTaskInt()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsGenericTaskInt), withValue: Task.FromResult(17))
                .Build();

            // Act
            var result = await proxy.MethodReturnsGenericTaskInt();

            // Assert
            result.Should().Be(17, because: "the method was stubbed with the value 17. ");

            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
        }

        [TestMethod]
        public async Task MethodReturnsTaskIntResult()
        {
            // Arrange
             var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskIntResult), withValue: Task.FromResult(13))
                .Build();

            // Act
            await proxy.MethodReturnsTaskIntResult();

            // Assert
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
        }

        [TestMethod]
        public async Task MethodReturnsGenerictaskIntNotIntercepted()
        {
            // Arrange, Act
            var result = await _originalImplementation.MethodReturnsGenericTaskInt();

            // Assert
            result.Should().Be(10, because: "the task should have completed by now. ");
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsGenericTaskInt)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public void MethodReturnsTaskButThrowsException()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskButThrowsException), Task.CompletedTask)
                .Build();

            // Act
            var task = proxy.MethodReturnsTaskButThrowsException();

            // Assert
            task.Should().NotBeNull(because: "the exception was not thrown because we were stubbed. ");
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void MethodReturnsTaskButThrowsExceptionNotIntercepted()
        {
            // Arrange, Act, Assert
            _originalImplementation.MethodReturnsTaskButThrowsException().Wait();
        }

        [TestMethod]
        public async Task MethodReturnsTaskVoidAsync()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoidAsync), withValue: Task.CompletedTask)
                .Build();

            // Act
            await proxy.MethodReturnsTaskVoidAsync();

            // Assert
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
        }

        [TestMethod]
        public async Task MethodReturnsTaskVoidAsyncNotIntercepted()
        {
            // Arrange, Act
            await _originalImplementation.MethodReturnsTaskVoidAsync();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoidAsync)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public async Task MethodReturnsGenericTaskAsync()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsGenericTaskAsync), withValue: Task.FromResult(new Product[1] { 
                new Product()
                {
                    Name = "MockedName1",
                    Description = "MockedDescription1"
                }
            } as IEnumerable<Product>))
           .Build();

            // Act
            var result = await proxy.MethodReturnsGenericTaskAsync();

            // Assert
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");

            var products = result as IEnumerable<Product>;
            products.Should().NotBeNull(because: "we have stubbed a single product. ");

            products.Count().Should().Be(1, because: "the stubbed to return one product ");
            products.First().Name.Should().Be("MockedName1", because: "that is the product name ");
            products.First().Description.Should().Be("MockedDescription1", because: "that is the product name ");
        }

        [TestMethod]
        public async Task MethodReturnsGenericTaskNotInterceptedAsync()
        {
            // Arrange, Act
            var result = await _originalImplementation.MethodReturnsGenericTaskAsync();

            // Assert
            result.Count().Should().Be(2, because: "that is how many products are returned in the real method. ");

            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsGenericTaskAsync)}", because: "the method should have fully completed without any callbacks. ");
        }
    }
}
