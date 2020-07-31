using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class AfterExecutionOfAsyncTests
    {
        [TestClass]
        public class Interception_Is_Invoked : AfterExecutionOfTestBase
        {
            [TestMethod]
            public void When_Async_Method_Is_Void()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.AsyncIsVoid), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                proxy.AsyncIsVoid();
                System.Threading.Thread.Sleep(20);

                // Assert
                AssertReturnValue(nameof(IAfterExecutionMethodSignatures.AsyncIsVoid), false, inResult: store);
            }

            [TestMethod]
            public async Task When_Async_Method_Returns_A_Void_Task()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.AsyncReturnsVoidTask), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                await proxy.AsyncReturnsVoidTask();

                // Assert
                AssertReturnValueIsVoid(nameof(IAfterExecutionMethodSignatures.AsyncReturnsVoidTask), inResult: store);
            }

            [TestMethod]
            public async Task When_Async_Method_Returns_A_Generic_Task_Result_Of_IEnumerable()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.AsyncReturnsGenericTaskResult), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                await proxy.AsyncReturnsGenericTaskResult();

                AssertReturnValue(nameof(IAfterExecutionMethodSignatures.AsyncReturnsGenericTaskResult), hasReturnValue: true, inResult: store);

                var result = store.ReturnValue as IEnumerable<Product>;
                result.Should().NotBeNull(because: "the async method returns the enumerable collection. ");
                result.Count().Should().Be(2, because: "two products are returned from the method. ");

                result.First().Name.Should().Be("Name1", because: "that is the name of the product. ");
                result.First().Description.Should().Be("Description1", because: "that is the description of the product. ");
                result.Last().Name.Should().Be("Name2", because: "that is the name of the product. ");
                result.Last().Description.Should().Be("Description2", because: "that is the description of the product. ");
            }

            [TestMethod]
            public void When_A_Task_Awaiter_Is_Provided()
            {
                // Arrange
                var taskWaiterCalled = false;

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.AsyncReturnsVoidTask), andCallbackWith: result =>
                {
                })
                .WithTaskAwaiter(task =>
                {
                    taskWaiterCalled = true;
                    task.Wait();
                })
                .Build();

                // Act
                var task = proxy.AsyncReturnsVoidTask();

                taskWaiterCalled.Should().BeTrue(because: "we provided our custom task waiter. ");
            }
        }

        [TestClass]
        public class Has_No_Effect_On_Behavior_If_Not_Configured : AfterExecutionOfTestBase
        {
            [TestMethod]
            public async Task When_Async_Method_Returns_A_Void_Task()
            {
                // Arrange, Act
                await _originalImplementation.AsyncReturnsVoidTask();

                // Assert
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.AsyncReturnsVoidTask)}", because: "the method should have fully completed without any callbacks. ");
            }
        }
    }
}
