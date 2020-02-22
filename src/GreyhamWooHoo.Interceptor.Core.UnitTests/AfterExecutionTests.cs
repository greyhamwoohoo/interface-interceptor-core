using FluentAssertions;
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
        public void MethodIsVoid()
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
        public void MethodReturnsIntNotIntercepted()
        {
            // Arrange, Act
            _originalImplementation.MethodReturnsInt();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionTestInterface.MethodReturnsInt)}", because: "the method should have fully completed without any callbacks. ");
        }
    }
}
