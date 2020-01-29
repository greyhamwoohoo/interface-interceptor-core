using FluentAssertions;
using FluentAssertions.Execution;
using GreyhamWooHoo.Interceptor.Core.Builders;
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
        public void VoidTaskMethod()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoid), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            var task = proxy.MethodReturnsTaskVoid();

            task.Wait();

            // Assert
            AssertReturnValueIsVoid(nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoid), inResult: store);
        }

        [TestMethod]
        public void VoidTaskMethodNotIntercepted()
        {
            // Arrange, Act
            _originalImplementation.MethodReturnsTaskVoid().Wait();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsTaskVoid)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public void IntTaskMethod()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsTaskInt), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            var task = proxy.MethodReturnsTaskInt();

            // Assert
            task.Result.Should().Be(10, because: "the task should have completed by now. ");

            AssertReturnValue(nameof(IAfterExecutionTestInterface.MethodReturnsTaskInt), isValue: 10, inResult: store);
        }

        [TestMethod]
        public void IntTaskMethodNotIntercepted()
        {
            // Arrange, Act
            var task = _originalImplementation.MethodReturnsTaskInt();
            task.Wait();

            // Assert
            task.Result.Should().Be(10, because: "the task should have completed by now. ");
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsTaskInt)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TaskMethodThrowsException()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodTaskThrowsException), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            var task = proxy.MethodTaskThrowsException();
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TaskMethodThrowsExceptionNotIntercepted()
        {
            // Arrange, Act, Assert
            _originalImplementation.MethodTaskThrowsException().Wait();
        }

        [TestMethod]
        public void AsyncVoidTaskMethod()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsAsyncVoidTask), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            var task = proxy.MethodReturnsAsyncVoidTask();

            task.Wait();

            // Assert
            AssertReturnValue(nameof(IAfterExecutionTestInterface.MethodReturnsAsyncVoidTask), true, inResult: store);

            // Design Decision: store.ReturnValue is undefined but not null in the case of a method signature like 'async Task TheMethod()'
            store.ReturnValue.Should().NotBeNull(because: "this is a design decision - until I work out how to distinguish async Task DoMethod() and Task DoMethod()");
        }

        [TestMethod]
        public async Task AsyncVoidTaskMethodNotIntercepted()
        {
            // Arrange, Act
            await _originalImplementation.MethodReturnsAsyncVoidTask();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsAsyncVoidTask)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public void AsyncGenericTaskMethod()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsAsyncGenericTask), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            var task = proxy.MethodReturnsAsyncGenericTask();

            task.Wait();

            AssertReturnValue(nameof(IAfterExecutionTestInterface.MethodReturnsAsyncGenericTask), hasReturnValue: true, inResult: store);
            
            var result = store.ReturnValue as IEnumerable<Product>;
            result.Should().NotBeNull(because: "the async method returns the enumerable collection. ");
            result.Count().Should().Be(2, because: "two products are returned from the method. ");

            result.First().Name.Should().Be("Name1", because: "that is the name of the product. ");
            result.First().Description.Should().Be("Description1", because: "that is the description of the product. ");
            result.Last().Name.Should().Be("Name2", because: "that is the name of the product. ");
            result.Last().Description.Should().Be("Description2", because: "that is the description of the product. ");
        }
    }
}
