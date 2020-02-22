using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Contracts;
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
    public class AfterExecutionTaskTests : AfterTestBase
    {
        [TestMethod]
        public async Task MethodReturnsTaskVoid()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoid), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            await proxy.MethodReturnsTaskVoid();

            // Assert
            AssertReturnValueIsVoid(nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoid), inResult: store);
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
        public async Task MethodReturnsTaskIntResult()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskIntResult), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            await proxy.MethodReturnsTaskIntResult();

            // Assert
            AssertReturnValue(nameof(IAfterExecutionTestInterface.MethodReturnsTaskIntResult), isValue: 25, inResult: store);
        }

        [TestMethod]
        public async Task MethodReturnsTaskIntResultNotIntercepted()
        {
            // Arrange, Act
            await _originalImplementation.MethodReturnsTaskIntResult();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsTaskIntResult)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public async Task MethodReturnsGenericTaskInt()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsGenericTaskInt), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            var result = await proxy.MethodReturnsGenericTaskInt();

            // Assert
            result.Should().Be(10, because: "the task should have completed by now. ");

            AssertReturnValue(nameof(IAfterExecutionTestInterface.MethodReturnsGenericTaskInt), isValue: 10, inResult: store);
        }

        [TestMethod]
        public async Task MethodReturnsGenericTaskIntNotIntercepted()
        {
            // Arrange, Act
            var result = await _originalImplementation.MethodReturnsGenericTaskInt();

            // Assert
            result.Should().Be(10, because: "the task should have completed by now. ");
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsGenericTaskInt)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task MethodReturnsTaskButThrowsException()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskButThrowsException), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            await proxy.MethodReturnsTaskButThrowsException();
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
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoidAsync), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            await proxy.MethodReturnsTaskVoidAsync();

            // Assert
            AssertReturnValueIsVoid(nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoidAsync), inResult: store);
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
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsGenericTaskAsync), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            await proxy.MethodReturnsGenericTaskAsync();

            AssertReturnValue(nameof(IAfterExecutionTestInterface.MethodReturnsGenericTaskAsync), hasReturnValue: true, inResult: store);
            
            var result = store.ReturnValue as IEnumerable<Product>;
            result.Should().NotBeNull(because: "the async method returns the enumerable collection. ");
            result.Count().Should().Be(2, because: "two products are returned from the method. ");

            result.First().Name.Should().Be("Name1", because: "that is the name of the product. ");
            result.First().Description.Should().Be("Description1", because: "that is the description of the product. ");
            result.Last().Name.Should().Be("Name2", because: "that is the name of the product. ");
            result.Last().Description.Should().Be("Description2", because: "that is the description of the product. ");
        }

        [TestMethod]
        public void MethodReturnsVoidAsync()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsVoidAsync), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            proxy.MethodReturnsVoidAsync();
            System.Threading.Thread.Sleep(20);

            // Assert
            AssertReturnValue(nameof(IAfterExecutionTestInterface.MethodReturnsVoidAsync), false, inResult: store);
        }

        [TestMethod]
        public void TaskWaiterIsInvokedForTasks()
        {
            // Arrange
            var taskWaiterCalled = false;

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoidAsync), andCallbackWith: result =>
            {
            })
            .WithTaskAwaiter(task =>
            {
                taskWaiterCalled = true;
                task.Wait();
            })
            .Build();

            // Act
            var task = proxy.MethodReturnsTaskVoidAsync();

            taskWaiterCalled.Should().BeTrue(because: "we provided our custom task waiter. ");
        }
    }
}
