using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Contracts.Generic;
using GreyhamWooHoo.Interceptor.Core.UnitTests.BeforeExecution;
using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class IInterceptorProxyBuilderTests
    {
        protected readonly AfterExecutionTestImplementation _originalImplementation = new AfterExecutionTestImplementation();

        protected IInterceptorProxyBuilder _builder;

        [TestInitialize]
        public void SetupReturnValueTests()
        {
            _builder = new InterceptorProxyBuilder<IAfterExecutionTestInterface>() as IInterceptorProxyBuilder;
            _builder.For(_originalImplementation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenInstanceIsNull_WillThrow()
        {
            _builder.For(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenInstanceIsNotTypeOfT_WillThrow()
        {
            _builder.For(new Product());
        }

        [TestMethod]
        public void WhenBuild_ReturnsInterface()
        {
            // Arrange, Act
            var proxy = _builder.Build() as IAfterExecutionTestInterface;

            // Assert
            proxy.Should().NotBeNull(because: "the Proxy<T> parameter is Proxy<IAfterExecutionTestInterface> and should have been returned. ");
        }

        [TestMethod]
        public void WhenBuild_CanInvokeNonStubbedMethod()
        {
            // Arrange, Act
            var proxy = _builder.Build() as IAfterExecutionTestInterface;

            // Assert
            proxy.MethodReturnsInt().Should().Be(10, because: "the implemented method returns 10");
        }

        [TestMethod]
        public void WhenInterceptAfterExecutionOf_CallsBackAfterwards()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsInt), andCallbackWith: result =>
            {
                store = result;
            })
            .Build() as IAfterExecutionTestInterface;

            // Act
            var result = proxy.MethodReturnsInt();

            // Assert
            result.Should().Be(10, because: "the method returns 10");
            store.HasReturnValue.Should().BeTrue(because: "the method should returns 10");
            ((int)store.ReturnValue).Should().Be(10, because: "that is the value returned. ");
        }

        [TestMethod]
        public void WhenInterfaceBeforeExecutionOf_CallsBackBefore()
        {
            // Arrange
            var _originalImplementation = new BeforeExecutionTestImplementation();

            IInterceptorProxyBuilder<IBeforeExecutionTestInterface> builder = new InterceptorProxyBuilder<IBeforeExecutionTestInterface>()
                .For(_originalImplementation);

            // Arrange
            var args = default(object[]);
            var parameters = default(IDictionary<string, object>);
            var calledBack = false;

            var proxy = builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.MethodWithOneParameter), andCallBackWith: result =>
            {
                calledBack = true;
                args = result.Args;
                parameters = result.Parameters;
            })
            .Build() as IBeforeExecutionTestInterface;

            // Act
            proxy.MethodWithOneParameter(10);

            // Assert
            calledBack.Should().BeTrue(because: "the callback should have been invoked. ");
            ((int)parameters["parameter1"]).Should().Be(10, because: "the value 10 was passed in. ");
        }

        [TestMethod]
        public void WhenVoidIsStubbed_NoValueReturned()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsVoid))
                .Build() as IAfterExecutionTestInterface;

            // Act
            proxy.MethodReturnsVoid();

            // Assert
            _originalImplementation.Message.Should().BeNull(because: "the method was stubbed and did not execute. ");
        }

        [TestMethod]
        public void WhenIntIsStubbed_ReturnsStubbedValue()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsInt), withValue: 15)
                .Build() as IAfterExecutionTestInterface;

            // Act
            var result = proxy.MethodReturnsInt();

            // Assert
            result.Should().Be(15, because: "that is the stubbed value");
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
        }

        [TestMethod]
        public void WhenIntIsDynamicallyProvided_ReturnsStubbedValue()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsInt), dynamicValueProvider: callContext => 
            {
                return 123;
            })
            .Build() as IAfterExecutionTestInterface;

            // Act
            var result = proxy.MethodReturnsInt();

            // Assert
            result.Should().Be(123, because: "that is the stubbed value");
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
        }

        [TestMethod]
        public void WhenTaskAwaiterIsProvided_TaskWaiterIsCalled()
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
            .Build() as IAfterExecutionTestInterface;

            // Act
            var task = proxy.MethodReturnsTaskVoidAsync();

            taskWaiterCalled.Should().BeTrue(because: "we provided our custom task waiter. ");
        }
    }
}
