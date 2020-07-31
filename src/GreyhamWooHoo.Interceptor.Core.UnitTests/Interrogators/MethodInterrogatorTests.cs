using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Interrogators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.Interrogators
{
    [TestClass]
    public class MethodInterrogatorTests
    {
        IMethodInterrogator returnValueInterrogator;
        ReturnValueTestClass cut;

        [TestInitialize]
        public void SetupReturnValueInterrogatorTests()
        {
            cut = new ReturnValueTestClass();
            returnValueInterrogator = new MethodInterrogator();
        }

        [TestMethod]
        [DataRow(true, nameof(ReturnValueTestClass.MethodIsVoid))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsInt))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsTask))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsTaskGenericInt))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodIsVoid))]
        [DataRow(false, nameof(ReturnValueTestClass.AsyncMethodReturnsTask))]
        [DataRow(false, nameof(ReturnValueTestClass.AsyncMethodReturnsTaskGenericInt))]
        public void Knows_A_Method_Is_Void(bool isVoid, string methodName)
        {
            // Arrange
            var methodInfo = cut.GetType().GetMethod(methodName);

            // Act
            var result = returnValueInterrogator.IsVoid(methodInfo);

            // Assert
            result.Should().Be(isVoid);
        }

        [TestMethod]
        [DataRow(false, nameof(ReturnValueTestClass.MethodIsVoid))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsInt))]
        [DataRow(true, nameof(ReturnValueTestClass.MethodReturnsTask))]
        [DataRow(true, nameof(ReturnValueTestClass.MethodReturnsTaskGenericInt))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodReturnsTask))]
        [DataRow(false, nameof(ReturnValueTestClass.AsyncMethodIsVoid))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodReturnsTaskGenericInt))]
        public void Knows_A_Method_Returns_A_Task(bool isTask, string methodName)
        {
            // Arrange
            var methodInfo = cut.GetType().GetMethod(methodName);

            // Act
            var result = returnValueInterrogator.ReturnsTask(methodInfo);

            // Assert
            result.Should().Be(isTask);
        }

        [TestMethod]
        [DataRow(false, nameof(ReturnValueTestClass.MethodIsVoid))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsInt))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsTask))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsTaskGenericInt))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodReturnsTask))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodIsVoid))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodReturnsTaskGenericInt))]
        public void Knows_A_Method_Is_Async(bool isAsync, string methodName)
        {
            // Arrange
            var methodInfo = cut.GetType().GetMethod(methodName);

            // Act
            var result = returnValueInterrogator.IsAsync(methodInfo);

            // Assert
            result.Should().Be(isAsync);
        }

        [TestMethod]
        [DataRow(false, nameof(ReturnValueTestClass.MethodIsVoid))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsInt))]
        [DataRow(true, nameof(ReturnValueTestClass.MethodReturnsTask))]
        [DataRow(true, nameof(ReturnValueTestClass.MethodReturnsTaskGenericInt))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodReturnsTask))]
        [DataRow(false, nameof(ReturnValueTestClass.AsyncMethodIsVoid))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodReturnsTaskGenericInt))]
        public void Knows_A_Method_Is_Awaitable(bool isAwaitable, string methodName)
        {
            // Arrange
            var methodInfo = cut.GetType().GetMethod(methodName);

            // Act
            var result = returnValueInterrogator.IsAwaitable(methodInfo);

            // Assert
            result.Should().Be(isAwaitable);
        }

        [TestMethod]
        [DataRow(false, nameof(ReturnValueTestClass.MethodIsVoid))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsInt))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsTask))]
        [DataRow(true, nameof(ReturnValueTestClass.MethodReturnsTaskGenericInt))]
        [DataRow(false, nameof(ReturnValueTestClass.AsyncMethodReturnsTask))]
        [DataRow(false, nameof(ReturnValueTestClass.AsyncMethodIsVoid))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodReturnsTaskGenericInt))]
        public void Knows_A_Method_Returns_A_Generic_Task_Result(bool isGenericTask, string methodName)
        {
            // Arrange
            var methodInfo = cut.GetType().GetMethod(methodName);

            // Act
            var result = returnValueInterrogator.ReturnsGenericTask(methodInfo);

            // Assert
            result.Should().Be(isGenericTask);
        }
    }

    public class ReturnValueTestClass
    {
        public void MethodIsVoid() { }
        public int MethodReturnsInt() => 10;
        public Task MethodReturnsTask() => Task.CompletedTask;
        public Task<int> MethodReturnsTaskGenericInt() => Task.FromResult(10);
        public async void AsyncMethodIsVoid() => await Task.Run(() => Task.CompletedTask);
        public async Task AsyncMethodReturnsTask() => await Task.Run(() => Task.CompletedTask);
        public async Task<int> AsyncMethodReturnsTaskGenericInt() => await Task.FromResult(10);
    }
}
