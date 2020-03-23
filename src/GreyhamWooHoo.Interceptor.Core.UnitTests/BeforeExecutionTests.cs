using FluentAssertions;
using FluentAssertions.Execution;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.Contracts.Generic;
using GreyhamWooHoo.Interceptor.Core.UnitTests.BeforeExecution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class BeforeExecutionTests
    {
        private readonly BeforeExecutionTestImplementation _originalImplementation = new BeforeExecutionTestImplementation();

        private IInterceptorProxyBuilder<IBeforeExecutionTestInterface> _builder;

        [TestInitialize]
        public void SetupReturnValueTests()
        {
            _builder = new InterceptorProxyBuilder<IBeforeExecutionTestInterface>()
                .For(_originalImplementation);
        }

        [TestMethod]
        public void MethodWithNoParameters()
        {
            // Arrange
            var args = default(object[]);
            var parameters = default(IDictionary<string, object>);
            var calledBack = false;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.MethodWithNoParameters), andCallBackWith: result =>
            {
                calledBack = true;
                args = result.Args;
                parameters = result.Parameters;
            })
            .Build();

            // Act
            proxy.MethodWithNoParameters();

            // Assert
            using var scope = new AssertionScope();
            calledBack.Should().BeTrue(because: "the callback should have been invoked. ");

            args.Should().NotBeNull(because: "the callback will have been invoked with no arguments. ");
            args.Length.Should().Be(0, because: "the method has no parameters. ");

            parameters.Count.Should().Be(0, because: "the method has no parameters ");

            _originalImplementation.Message.Should().Be("Invoked", because: "it is set by the method after the calledback. ");
        }

        [TestMethod]
        public void MethodWithOneParameter()
        {
            // Arrange
            var args = default(object[]);
            var parameters = default(IDictionary<string, object>);
            var calledBack = false;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.MethodWithOneParameter), andCallBackWith: result =>
            {
                calledBack = true;
                args = result.Args;
                parameters = result.Parameters;
            })
            .Build();

            // Act
            proxy.MethodWithOneParameter(10);

            // Assert
            using var scope = new AssertionScope();
            calledBack.Should().BeTrue(because: "the callback should have been invoked. ");

            args.Should().NotBeNull(because: "the callback will have been invoked with one arguments. ");
            args.Length.Should().Be(1, because: "there is exactly one parameter in the method that was invoked. ");
            ((int)args[0]).Should().Be(10, because: "that is the value of the parameter passed in. ");

            parameters.Count.Should().Be(1, because: "the method has one parameter. ");
            ((int)parameters["parameter1"]).Should().Be(10, because: "that is the value of the one parameter. ");

            _originalImplementation.Message.Should().Be("Invoked: 10", because: "it is set by the method after the calledback. ");
        }

        [TestMethod]
        public void MethodWithTwoParameters()
        {
            // Arrange
            var args = default(object[]);
            var parameters = default(IDictionary<string, object>);
            var calledBack = false;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.MethodWithTwoParameters), andCallBackWith: result =>
            {
                calledBack = true;
                args = result.Args;
                parameters = result.Parameters;
            })
            .Build();

            // Act
            proxy.MethodWithTwoParameters(20, 30);

            // Assert
            using var scope = new AssertionScope();
            calledBack.Should().BeTrue(because: "the callback should have been invoked. ");

            args.Should().NotBeNull(because: "the callback will have been invoked with one arguments. ");
            args.Length.Should().Be(2, because: "there is exactly one parameter in the method that was invoked. ");
            ((int)args[0]).Should().Be(20, because: "that is the value of the parameter passed in. ");
            ((int)args[1]).Should().Be(30, because: "that is the value of the parameter passed in. ");

            parameters.Count.Should().Be(2, because: "the method has two parameters. ");
            ((int)parameters["parameter1"]).Should().Be(20, because: "that is the value of the first parameter. ");
            ((int)parameters["parameter2"]).Should().Be(30, because: "that is the value of the second parameter. ");

            _originalImplementation.Message.Should().Be("Invoked: 20 30", because: "it is set by the method after the calledback. ");
        }

        [TestMethod]
        public void ManyBeforeCallouts()
        {
            // Arrange
            var callback1 = false;
            var callback2 = false;

            var proxy = _builder
                .InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.MethodWithNoParameters), andCallBackWith: result =>
                {
                    callback1 = true;
                })
                .InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.MethodWithNoParameters), andCallBackWith: result =>
                {
                    callback2 = true;
                })
                .Build();

            // Act
            proxy.MethodWithNoParameters();
            
            // Assert
            callback1.Should().BeTrue(because: "the first OnBefore callout will be invoked. ");
            callback2.Should().BeTrue(because: "the second OnBefore callout will be invoked. ");
        }

        [TestMethod]
        public void WhenInterfaceHasTwoCallbacks_MethodIsCalledOnlyOnce()
        {
            // Arrange
            var methodWithNoParameters = 0;
            var methodWithOneParameter = 0;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.MethodWithNoParameters), andCallBackWith: result =>
            {
                methodWithNoParameters++;
            })
            .InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.MethodWithOneParameter), andCallBackWith: result =>
            {
                methodWithOneParameter++;
            }).Build();

            // Act 1
            proxy.MethodWithNoParameters();

            // Assert 1
            methodWithNoParameters.Should().Be(1, because: "this was the method that was explicitly invoked. ");
            methodWithOneParameter.Should().Be(0, because: "this method was not invoked. ");

            // Act 2
            proxy.MethodWithOneParameter(10);

            methodWithNoParameters.Should().Be(1, because: "this method was not invoked this time. ");
            methodWithOneParameter.Should().Be(1, because: "this method was invoke this time. ");
        }
    }
}
