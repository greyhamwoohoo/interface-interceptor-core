using FluentAssertions;
using FluentAssertions.Execution;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.UnitTests.BeforeExecution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class BeforeExecutionTests
    {
        private readonly BeforeExecutionTestImplementation _originalImplementation = new BeforeExecutionTestImplementation();

        private InterceptorProxyBuilder<IBeforeExecutionTestInterface> _builder;

        [TestInitialize]
        public void SetupReturnValueTests()
        {
            _builder = new InterceptorProxyBuilder<IBeforeExecutionTestInterface>()
                .For(_originalImplementation);
        }

        [TestMethod]
        public void MethodHasNoParameters()
        {
            // Arrange
            var args = default(object[]);
            var parameters = default(IDictionary<string, object>);
            var calledBack = false;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.TheMethodWithNoParameters), andCallBackWith: result =>
            {
                calledBack = true;
                args = result.Args;
                parameters = result.Parameters;
            })
            .Build();

            // Act
            proxy.TheMethodWithNoParameters();

            // Assert
            using var scope = new AssertionScope();
            calledBack.Should().BeTrue(because: "the callback should have been invoked. ");

            args.Should().NotBeNull(because: "the callback will have been invoked with no arguments. ");
            args.Length.Should().Be(0, because: "the method has no parameters. ");

            parameters.Count.Should().Be(0, because: "the method has no parameters ");

            _originalImplementation.Message.Should().Be("Invoked", because: "it is set by the method after the calledback. ");
        }

        [TestMethod]
        public void MethodHasOneParameter()
        {
            // Arrange
            var args = default(object[]);
            var parameters = default(IDictionary<string, object>);
            var calledBack = false;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.TheMethodWithOneParameter), andCallBackWith: result =>
            {
                calledBack = true;
                args = result.Args;
                parameters = result.Parameters;
            })
            .Build();

            // Act
            proxy.TheMethodWithOneParameter(10);

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
        public void MethodHasManyParameters()
        {
            // Arrange
            var args = default(object[]);
            var parameters = default(IDictionary<string, object>);
            var calledBack = false;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.TheMethodWithManyParameters), andCallBackWith: result =>
            {
                calledBack = true;
                args = result.Args;
                parameters = result.Parameters;
            })
            .Build();

            // Act
            proxy.TheMethodWithManyParameters(20, 30);

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
    }
}
