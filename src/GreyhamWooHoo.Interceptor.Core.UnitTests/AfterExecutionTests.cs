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
    public class AfterExecutionTests : AfterTestBase
    {
        [TestMethod]
        public void VoidMethod()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsVoid), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            proxy.MethodReturnsVoid();

            // Assert
            AssertReturnValueIsVoid(nameof(IAfterExecutionTestInterface.MethodReturnsVoid), inResult: store);
        }

        [TestMethod]
        public void VoidMethodNotIntercepted()
        {
            // Arrange, Act
            _originalImplementation.MethodReturnsVoid();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsVoid)}", because: "the method should have fully completed without any callbacks. ");
        }

        [TestMethod]
        public void IntMethod()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsInt), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            proxy.MethodReturnsInt();

            // Assert
            AssertReturnValue(nameof(IAfterExecutionTestInterface.MethodReturnsInt), isValue: 10, inResult: store);
        }

        [TestMethod]
        public void IntMethodNotIntercepted()
        {
            // Arrange, Act
            _originalImplementation.MethodReturnsInt();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsInt)}", because: "the method should have fully completed without any callbacks. ");
        }
    }
}
