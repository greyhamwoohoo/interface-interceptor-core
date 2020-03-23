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

        [TestMethod]
        public void ManyAfterCallouts()
        {
            // Arrange
            var callback1 = false;
            var callback2 = false;

            var proxy = _builder
                .InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsInt), andCallbackWith: result =>
                {
                    callback1 = true;
                })
                .InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodReturnsInt), andCallbackWith: result =>
                {
                    callback2 = true;
                })
                .Build();

            // Act
            proxy.MethodReturnsInt();

            // Assert
            callback1.Should().BeTrue(because: "the first OnAfter callout will be invoked. ");
            callback2.Should().BeTrue(because: "the second OnAfter callout will be invoked. ");
        }

        [TestMethod]
        public void MethodWithNoParameters()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodAsNoParameters), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            proxy.MethodAsNoParameters();

            // Assert
            store.Args.Should().NotBeNull(because: "when no parameters are provided, the Interceptor will return an empty collection. ");
            store.Parameters.Count.Should().Be(0, because: "there are no parameters for this method. ");
        }

        [TestMethod]
        public void MethodWithOneParameter()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodHasOneParameter), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            proxy.MethodHasOneParameter(theInt: 32);

            // Assert
            store.Args.Should().NotBeNull(because: "the arguments passed to the method should always be returned. ");
            store.Args.Length.Should().Be(1, because: "the method call has one argument. ");
            
            store.Parameters.Count.Should().Be(1, because: "there are no parameters for this method. ");
            var theIntParameter = (int) store.Parameters["theInt"];
            theIntParameter.Should().Be(32, because: "that is the value passed in. ");
        }

        [TestMethod]
        public void MethodWithTwoParameters()
        {
            // Arrange
            var store = default(IAfterExecutionResult);

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodHasTwoParameters), andCallbackWith: result =>
            {
                store = result;
            })
            .Build();

            // Act
            proxy.MethodHasTwoParameters(theString: "woo", theInt: 33);

            // Assert
            store.Args.Should().NotBeNull(because: "the arguments passed to the method should always be returned. ");
            store.Args.Length.Should().Be(2, because: "the method call has two arguments. ");

            store.Parameters.Count.Should().Be(2, because: "there are no parameters for this method. ");
            
            var theIntParameter = (int)store.Parameters["theInt"];
            theIntParameter.Should().Be(33, because: "that is the value passed in. ");

            var theStringParameter = (string)store.Parameters["theString"];
            theStringParameter.Should().Be("woo", because: "that is the value passed in. ");
        }

        [TestMethod]
        public void WhenInterfaceHasTwoCallbacks_MethodIsCalledOnlyOnce()
        {
            // Arrange
            var methodAsNoParametersCount = 0;
            var methodHasOneParametercount = 0;

            var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodAsNoParameters), andCallbackWith: result =>
            {
                methodAsNoParametersCount++;
            })
            .InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionTestInterface.MethodHasOneParameter), andCallbackWith: result =>
            {
                methodHasOneParametercount++;
            }).Build();

            // Act 1
            proxy.MethodAsNoParameters();

            // Assert 1
            methodAsNoParametersCount.Should().Be(1, because: "this was the method that was explicitly invoked. ");
            methodHasOneParametercount.Should().Be(0, because: "this method was not invoked. ");

        }
    }
}
