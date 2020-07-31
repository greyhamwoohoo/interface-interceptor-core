using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Contracts.Generic;
using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.Builders
{
    /// <summary>
    /// The IInterceptorProxyBuilder is the non-generic interface around the builder. 
    /// 
    /// The test coverage only needs to be adequate to ensure that calls are passed through to the generic builder. 
    /// </summary>
    [TestClass]
    public class IInterceptorProxyBuilderTests
    {
        protected readonly AfterExecutionMethodSignatures _originalImplementation = new AfterExecutionMethodSignatures();

        protected IInterceptorProxyBuilder _builder;

        [TestInitialize]
        public void SetupInterceptorProxyBuilderTests()
        {
            _builder = new InterceptorProxyBuilder<IAfterExecutionMethodSignatures>() as IInterceptorProxyBuilder;
            _builder.For(_originalImplementation);
        }

        [TestClass]
        public class Parameter_Validation : IInterceptorProxyBuilderTests
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void When_Instance_Is_Null_Will_Throw()
            {
                _builder.For(null);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void When_Instance_Is_Not_TypeOfT_Will_Throw()
            {
                _builder.For(new Product());
            }
        }

        [TestClass]
        public class Can_Build_An_Interceptor : IInterceptorProxyBuilderTests
        {
            [TestMethod]
            public void That_Exposes_The_Original_Interface()
            {
                // Arrange, Act
                var proxy = _builder.Build() as IAfterExecutionMethodSignatures;

                // Assert
                proxy.Should().NotBeNull(because: "the Proxy<T> parameter is Proxy<IAfterExecutionTestInterface> and should have been returned. ");
            }

            [TestMethod]
            public void That_Preserves_Original_Implementation()
            {
                // Arrange, Act
                var proxy = _builder.Build() as IAfterExecutionMethodSignatures;

                // Assert
                proxy.ReturnsIntWithValue10().Should().Be(10, because: "the implemented method returns 10");
            }

            [TestMethod]
            public void That_Supports_Callbacks_After_A_Method_Is_Invoked()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build() as IAfterExecutionMethodSignatures;

                // Act
                var result = proxy.ReturnsIntWithValue10();

                // Assert
                result.Should().Be(10, because: "the method returns 10");
                store.HasReturnValue.Should().BeTrue(because: "the method should returns 10");
                ((int)store.ReturnValue).Should().Be(10, because: "that is the value returned. ");
            }

            [TestMethod]
            public void That_Supports_Callbacks_Before_A_Method_Is_Invoked()
            {
                // Arrange
                var _originalImplementation = new BeforeExecutionMethodSignatures();

                IInterceptorProxyBuilder<IBeforeExecutionMethodSignatures> builder = new InterceptorProxyBuilder<IBeforeExecutionMethodSignatures>()
                    .For(_originalImplementation);

                // Arrange
                var args = default(object[]);
                var parameters = default(IDictionary<string, object>);
                var calledBack = false;

                var proxy = builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionMethodSignatures.HasOneParameter), andCallBackWith: result =>
                {
                    calledBack = true;
                    args = result.Args;
                    parameters = result.Parameters;
                })
                .Build() as IBeforeExecutionMethodSignatures;

                // Act
                proxy.HasOneParameter(10);

                // Assert
                calledBack.Should().BeTrue(because: "the callback should have been invoked. ");
                ((int)parameters["parameter1"]).Should().Be(10, because: "the value 10 was passed in. ");
            }

            [TestMethod]
            public void That_Supports_Stubbing_Void_Methods()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.IsVoid))
                    .Build() as IAfterExecutionMethodSignatures;

                // Act
                proxy.IsVoid();

                // Assert
                _originalImplementation.Message.Should().BeNull(because: "the method was stubbed and did not execute. ");
            }

            [TestMethod]
            public void That_Supports_Stubbing_Methods_That_Return_A_Constant_Primitive_Type()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10), withValue: 15)
                    .Build() as IAfterExecutionMethodSignatures;

                // Act
                var result = proxy.ReturnsIntWithValue10();

                // Assert
                result.Should().Be(15, because: "that is the stubbed value");
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
            }

            [TestMethod]
            public void That_Supports_Stubbing_Methods_That_Return_A_Dynamic_Primitive_Type()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10), dynamicValueProvider: callContext =>
                {
                    return 123;
                })
                .Build() as IAfterExecutionMethodSignatures;

                // Act
                var result = proxy.ReturnsIntWithValue10();

                // Assert
                result.Should().Be(123, because: "that is the stubbed value");
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
            }

            [TestMethod]
            public void That_Supports_Stubbing_Async_Methods_With_A_Custom_Task_Awaiter()
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
                .Build() as IAfterExecutionMethodSignatures;

                // Act
                var task = proxy.AsyncReturnsVoidTask();

                taskWaiterCalled.Should().BeTrue(because: "we provided our custom task waiter. ");
            }
        }
    }
}
