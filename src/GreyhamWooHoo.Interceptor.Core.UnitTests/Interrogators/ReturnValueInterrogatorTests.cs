using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Interrogators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.Interrogators
{
    [TestClass]
    public class ReturnValueInterrogatorTests
    {
        IReturnValueInterrogator returnValueInterrogator;
        ReturnValueTestClass cut;

        [TestInitialize]
        public void setupReturnValueInterrogatorTests()
        {
            cut = new ReturnValueTestClass();
            returnValueInterrogator = new ReturnValueInterrogator();
        }

        [TestMethod]
        [DataRow(true, nameof(ReturnValueTestClass.MethodIsVoid))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsInt))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsTask))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsTaskGenericInt))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodIsVoid))]
        [DataRow(false, nameof(ReturnValueTestClass.AsyncMethodReturnsTask))]
        [DataRow(false, nameof(ReturnValueTestClass.AsyncMethodReturnsTaskGenericInt))]
        public void MethodIsVoid(bool isVoid, string methodName)
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
        public void MethodReturnsTask(bool isTask, string methodName)
        {
            // Arrange
            var methodInfo = cut.GetType().GetMethod(methodName);

            // Act
            var result = returnValueInterrogator.IsTask(methodInfo);

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
        public void MethodIsAsync(bool isTask, string methodName)
        {
            // Arrange
            var methodInfo = cut.GetType().GetMethod(methodName);

            // Act
            var result = returnValueInterrogator.IsAsync(methodInfo);

            // Assert
            result.Should().Be(isTask);
        }

        [TestMethod]
        [DataRow(false, nameof(ReturnValueTestClass.MethodIsVoid))]
        [DataRow(false, nameof(ReturnValueTestClass.MethodReturnsInt))]
        [DataRow(true, nameof(ReturnValueTestClass.MethodReturnsTask))]
        [DataRow(true, nameof(ReturnValueTestClass.MethodReturnsTaskGenericInt))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodReturnsTask))]
        [DataRow(false, nameof(ReturnValueTestClass.AsyncMethodIsVoid))]
        [DataRow(true, nameof(ReturnValueTestClass.AsyncMethodReturnsTaskGenericInt))]
        public void MethodIsAwaitable(bool isTask, string methodName)
        {
            // Arrange
            var methodInfo = cut.GetType().GetMethod(methodName);

            // Act
            var result = returnValueInterrogator.IsAwaitable(methodInfo);

            // Assert
            result.Should().Be(isTask);
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
