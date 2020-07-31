using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class AfterExecutionOfTaskTests
    {
        [TestClass]
        public class Interception_Is_Invoked : AfterExecutionOfTestBase
        {
            [TestMethod]
            public async Task When_Method_Returns_Task_That_Is_Void()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsTaskThatIsVoid), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                await proxy.ReturnsTaskThatIsVoid();

                // Assert
                AssertReturnValueIsVoid(nameof(IAfterExecutionMethodSignatures.ReturnsTaskThatIsVoid), inResult: store);
            }

            [TestMethod]
            public async Task When_Method_Returns_Task_Result_With_Value_Types()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsTaskResultThatIsAnIntWithValue25), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                await proxy.ReturnsTaskResultThatIsAnIntWithValue25();

                // Assert
                AssertReturnValue(nameof(IAfterExecutionMethodSignatures.ReturnsTaskResultThatIsAnIntWithValue25), isValue: 25, inResult: store);
            }

            [TestMethod]
            public async Task When_Method_Returns_Generic_Task_Result_Of_Primitive_Types()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsGenericTaskResultThatIsAnIntWithValue10), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                var result = await proxy.ReturnsGenericTaskResultThatIsAnIntWithValue10();

                // Assert
                result.Should().Be(10, because: "the task should have completed by now. ");

                AssertReturnValue(nameof(IAfterExecutionMethodSignatures.ReturnsGenericTaskResultThatIsAnIntWithValue10), isValue: 10, inResult: store);
            }

            [TestMethod]
            [ExpectedException(typeof(AggregateException))]
            public async Task When_A_Method_Is_Supposed_To_Return_A_Task_But_Throws_An_Exception_Instead()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsTaskButThrowsAnExceptionInstead), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                await proxy.ReturnsTaskButThrowsAnExceptionInstead();
            }
        }

        [TestClass]
        public class Has_No_Effect_On_Behavior_If_Not_Configured : AfterExecutionOfTestBase
        {
            [TestMethod]
            [ExpectedException(typeof(AggregateException))]
            public void When_A_Method_Is_Supposed_To_Return_A_Task_But_Throws_An_Exception_Instead()
            {
                // Arrange, Act, Assert
                _originalImplementation.ReturnsTaskButThrowsAnExceptionInstead().Wait();
            }

            [TestMethod]
            public async Task When_Method_Returns_Generic_Task_Result_Of_Primitive_Types()
            {
                // Arrange, Act
                var result = await _originalImplementation.ReturnsGenericTaskResultThatIsAnIntWithValue10();

                // Assert
                result.Should().Be(10, because: "the task should have completed by now. ");
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.ReturnsGenericTaskResultThatIsAnIntWithValue10)}", because: "the method should have fully completed without any callbacks. ");
            }

            [TestMethod]
            public async Task When_Method_Returns_Task_Result_With_Value_Types()
            {
                // Arrange, Act
                await _originalImplementation.ReturnsTaskResultThatIsAnIntWithValue25();

                // Assert
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.ReturnsTaskResultThatIsAnIntWithValue25)}", because: "the method should have fully completed without any callbacks. ");
            }

            [TestMethod]
            public async Task When_Method_Returns_Task_That_Is_Void()
            {
                // Arrange, Act
                await _originalImplementation.ReturnsTaskThatIsVoid();

                // Assert
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.ReturnsTaskThatIsVoid)}", because: "the method should have fully completed without any callbacks. ");
            }
        }
    }
}
