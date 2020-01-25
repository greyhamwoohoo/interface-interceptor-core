using FluentAssertions;
using FluentAssertions.Execution;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class StubExecutionTests
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
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.TheVoidMethod))
                .Build();

            // Act
            proxy.TheVoidMethod();

            // Assert
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
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
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.TheIntMethod), withValue: 15)
                .Build();

            // Act
            var result = proxy.TheIntMethod();

            // Assert
            result.Should().Be(15, because: "that is the stubbed value");
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
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
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.TheTaskVoidMethod), Task.CompletedTask)
                .Build();

            // Act
            var task = proxy.TheTaskVoidMethod();
            task.Wait();

            // Assert
            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
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
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.TheTaskIntMethod), withValue: Task.FromResult(17))
                .Build();

            // Act
            var task = proxy.TheTaskIntMethod();

            // Assert
            task.Result.Should().Be(17, because: "the method was stubbed with the value 17. ");

            _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
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
        public void TaskThrowsException()
        {
            // Arrange
            var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionTestInterface.TheExceptionTaskMethod), Task.CompletedTask)
                .Build();

            // Act
            var task = proxy.TheExceptionTaskMethod();

            // Assert
            task.Should().NotBeNull(because: "the exception was not thrown because we were stubbed. ");
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TaskThrowExceptionNotIntercepted()
        {
            // Arrange, Act, Assert
            _originalImplementation.TheExceptionTaskMethod().Wait();
        }
    }
}
