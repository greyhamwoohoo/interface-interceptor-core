using FluentAssertions;
using FluentAssertions.Execution;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.Contracts.Generic;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    /// <summary>
    /// The Interceptor can invoke a callback before .Net invokes the implementation behind an interface. 
    /// </summary>
    [TestClass]
    public class BeforeExecutionOfTests
    {
        private readonly BeforeExecutionMethodSignatures _originalImplementation = new BeforeExecutionMethodSignatures();

        private IInterceptorProxyBuilder<IBeforeExecutionMethodSignatures> _builder;

        [TestInitialize]
        public void SetupOnBeforeExecutionofTests()
        {
            _builder = new InterceptorProxyBuilder<IBeforeExecutionMethodSignatures>()
                .For(_originalImplementation);
        }

        [TestClass]
        public class Interceptor_Is_Invoked : BeforeExecutionOfTests
        {
            [TestMethod]
            public void When_Method_Has_No_Parameters()
            {
                // Arrange
                var args = default(object[]);
                var parameters = default(IDictionary<string, object>);
                var calledBack = false;

                var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionMethodSignatures.HasNoParameters), andCallBackWith: result =>
                {
                    calledBack = true;
                    args = result.Args;
                    parameters = result.Parameters;
                })
                .Build();

                // Act
                proxy.HasNoParameters();

                // Assert
                using var scope = new AssertionScope();
                calledBack.Should().BeTrue(because: "the callback should have been invoked. ");

                args.Should().NotBeNull(because: "the callback will have been invoked with no arguments. ");
                args.Length.Should().Be(0, because: "the method has no parameters. ");

                parameters.Count.Should().Be(0, because: "the method has no parameters ");

                _originalImplementation.Message.Should().Be("Invoked", because: "it is set by the method after the calledback. ");
            }

            [TestMethod]
            public void When_Method_Has_One_Parameter()
            {
                // Arrange
                var args = default(object[]);
                var parameters = default(IDictionary<string, object>);
                var calledBack = false;

                var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionMethodSignatures.HasOneParameter), andCallBackWith: result =>
                {
                    calledBack = true;
                    args = result.Args;
                    parameters = result.Parameters;
                })
                .Build();

                // Act
                proxy.HasOneParameter(10);

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
            public void When_Method_Has_Two_Parameters()
            {
                // Arrange
                var args = default(object[]);
                var parameters = default(IDictionary<string, object>);
                var calledBack = false;

                var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionMethodSignatures.HasTwoParameters), andCallBackWith: result =>
                {
                    calledBack = true;
                    args = result.Args;
                    parameters = result.Parameters;
                })
                .Build();

                // Act
                proxy.HasTwoParameters(20, 30);

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
            public void Two_Times_When_A_Method_Has_Two_Interceptions()
            {
                // Arrange
                var callback1 = false;
                var callback2 = false;

                var proxy = _builder
                    .InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionMethodSignatures.HasNoParameters), andCallBackWith: result =>
                    {
                        callback1 = true;
                    })
                    .InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionMethodSignatures.HasNoParameters), andCallBackWith: result =>
                    {
                        callback2 = true;
                    })
                    .Build();

                // Act
                proxy.HasNoParameters();

                // Assert
                callback1.Should().BeTrue(because: "the first OnBefore callout will be invoked. ");
                callback2.Should().BeTrue(because: "the second OnBefore callout will be invoked. ");
            }

            [TestMethod]
            public void On_Intercepted_Methods_Only()
            {
                // Arrange
                var methodWithNoParameters = 0;
                var methodWithOneParameter = 0;

                var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionMethodSignatures.HasNoParameters), andCallBackWith: result =>
                {
                    methodWithNoParameters++;
                })
                .InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionMethodSignatures.HasOneParameter), andCallBackWith: result =>
                {
                    methodWithOneParameter++;
                }).Build();

                // Act 1
                proxy.HasNoParameters();

                // Assert 1
                methodWithNoParameters.Should().Be(1, because: "this was the method that was explicitly invoked. ");
                methodWithOneParameter.Should().Be(0, because: "this method was not invoked. ");

                // Act 2
                proxy.HasOneParameter(10);

                methodWithNoParameters.Should().Be(1, because: "this method was not invoked this time. ");
                methodWithOneParameter.Should().Be(1, because: "this method was invoke this time. ");
            }
        }
    }
}
