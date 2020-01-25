using FluentAssertions;
using FluentAssertions.Execution;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class AfterExecutionTests
    {
        private readonly AfterExecutionTestImplementation _originalImplementation = new AfterExecutionTestImplementation();

        private InterceptorProxyBuilder<IAfterExecutionTestInterface> _builder;

        [TestInitialize]
        public void SetupReturnValueTests()
        {
            _builder = new InterceptorProxyBuilder<IAfterExecutionTestInterface>()
                .For(_originalImplementation);
        }

        [TestMethod]
        public void Void()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.TheVoidMethod), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            proxy.TheVoidMethod();

            // Assert
            AssertReturnValueIsVoid(nameof(IAfterExecutionTestInterface.TheVoidMethod), inResult: store);
        }

        [TestMethod]
        public void VoidNotIntercepted()
        {
            // Arrange, Act
            _originalImplementation.TheVoidMethod();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.TheVoidMethod)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public void PrimitiveInt()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.TheIntMethod), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            proxy.TheIntMethod();

            // Assert
            AssertReturnValue(nameof(IAfterExecutionTestInterface.TheIntMethod), isValue: 10, inResult: store);
        }

        [TestMethod]
        public void PrimitiveIntNotIntercepted()
        {
            // Arrange, Act
            _originalImplementation.TheIntMethod();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.TheIntMethod)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public void TaskVoid()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.TheTaskVoidMethod), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            var task = proxy.TheTaskVoidMethod();

            task.Wait();

            // Assert
            AssertReturnValueIsVoid(nameof(IAfterExecutionTestInterface.TheTaskVoidMethod), inResult: store);
        }

        [TestMethod]
        public void TaskVoidNotIntercepted()
        {
            // Arrange, Act
            _originalImplementation.TheTaskVoidMethod().Wait();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.TheTaskVoidMethod)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public void TaskPrimitiveInt()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.TheTaskIntMethod), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            var task = proxy.TheTaskIntMethod();

            // Assert
            task.Result.Should().Be(10, because: "the task should have completed by now. ");

            AssertReturnValue(nameof(IAfterExecutionTestInterface.TheTaskIntMethod), isValue: 10, inResult: store);
        }

        [TestMethod]
        public void TaskPrimitiveIntNotIntercepted()
        {
            // Arrange, Act
            var task = _originalImplementation.TheTaskIntMethod();
            task.Wait();

            // Assert
            task.Result.Should().Be(10, because: "the task should have completed by now. ");
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.TheTaskIntMethod)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TaskThrowsException()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.TheExceptionTaskMethod), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            var task = proxy.TheExceptionTaskMethod();
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TaskThrowExceptionNotIntercepted()
        {
            // Arrange, Act, Assert
            _originalImplementation.TheExceptionTaskMethod().Wait();
        }

        private void AssertReturnValue(string forMethod, int isValue, IAfterExecutionResult inResult)
        {
            _originalImplementation.Message.Should().Be($"Invoked: {forMethod}", because: "the method should have been invoked before the callback. ");

            inResult.Should().NotBeNull(because: "the callback should have been invoked with the return value result. ");
            using (var scope = new AssertionScope())
            {
                inResult.HasReturnValue.Should().BeTrue(because: "the method is expected to return a primitive type. ");
                inResult.ReturnValue.Should().Be(isValue, because: "that is the hard coded value returned from the method. ");
            }

            inResult.Rule.Should().NotBeNull(because: "the intercept rule should always be passed to the callback. ");
            inResult.Rule.MethodName.Should().Be(forMethod);
        }

        private void AssertReturnValueIsVoid(string forMethod, IAfterExecutionResult inResult)
        {
            _originalImplementation.Message.Should().Be($"Invoked: {forMethod}", because: "the method should have been invoked before the callback. ");

            inResult.Should().NotBeNull(because: "the callback should have been invoked with the return value result. ");
            using (var scope = new AssertionScope())
            {
                inResult.HasReturnValue.Should().BeFalse(because: "a void method has no return value. ");
                inResult.ReturnValue.Should().BeNull(because: "by design, we ensure that a void method will return null; use HasReturnValue as a discriminator. ");
            }

            inResult.Rule.Should().NotBeNull(because: "the intercept rule should always be passed to the callback. ");
            inResult.Rule.MethodName.Should().Be(forMethod);
        }
    }
}
