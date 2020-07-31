using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.UnitTests.Models;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{

    [TestClass]
    public class StubExecutionTests
    {
        [TestClass]
        public class Interceptor_Can_Stub : AfterExecutionOfTestBase
        {
            [TestMethod]
            public void When_Method_Is_Void()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.IsVoid))
                    .Build();

                // Act
                proxy.IsVoid();

                // Assert
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
            }


            [TestMethod]
            public void When_Method_Has_No_Parameters_Can_Stub_With_Dynamic_Value()
            {
                IMethodCallContext context = default;

                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.HasNoParameters), dynamicValueProvider: callContext =>
                {
                    context = callContext;
                    return 25;
                })
                    .Build();

                // Act
                var result = proxy.HasNoParameters();

                // Assert
                result.Should().Be(25, because: "that is the stubbed value");
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");

                context.Args.Length.Should().Be(0, because: "this method has no parameters. ");
                context.Parameters.Count.Should().Be(0, because: "this method has no parameters. ");
            }

            [TestMethod]
            public void When_Method_Has_One_Parameters_Can_Stub_With_Dynamic_Value()
            {
                IMethodCallContext context = default;

                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.HasOneParameter), dynamicValueProvider: callContext =>
                {
                    context = callContext;
                    return 35;
                })
                    .Build();

                // Act
                var result = proxy.HasOneParameter(65);

                // Assert
                result.Should().Be(35, because: "that is the stubbed value");
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");

                context.Args.Length.Should().Be(1, because: "this method has one parameter. ");
                ((int)context.Args[0]).Should().Be(65, because: "that is the value passed in. ");

                context.Parameters.Count.Should().Be(1, because: "this method has one parameter. ");
                ((int)context.Parameters["theInt"]).Should().Be(65, because: "this method has one parameter. ");
            }

            [TestMethod]
            public void When_Method_Has_Two_Parameters_Can_Stub_With_Dynamic_Value()
            {
                IMethodCallContext context = default;

                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.HasTwoParameters), dynamicValueProvider: callContext =>
                {
                    context = callContext;
                    return 14;
                })
                .Build();

                // Act
                var result = proxy.HasTwoParameters(theString: "theS", theInt: 98);

                // Assert
                result.Should().Be(14, because: "that is the stubbed value");
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");

                context.Args.Length.Should().Be(2, because: "this method has two parameters. ");
                ((string)context.Args[0]).Should().Be("theS", because: "that is the value passed in. ");
                ((int)context.Args[1]).Should().Be(98, because: "that is the value passed in. ");

                context.Parameters.Count.Should().Be(2, because: "this method has two parameters. ");
                ((int)context.Parameters["theInt"]).Should().Be(98, because: "this is the argument passed in. ");
                ((string)context.Parameters["theString"]).Should().Be("theS", because: "this is the argument passed in. ");
            }

            [TestMethod]
            public void When_Method_Returns_A_Value()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10), withValue: 15)
                    .Build();

                // Act
                var result = proxy.ReturnsIntWithValue10();

                // Assert
                result.Should().Be(15, because: "that is the stubbed value");
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
            }

            [TestMethod]
            public async Task When_Method_Returns_A_Void_Task()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsTaskThatIsVoid), Task.CompletedTask)
                    .Build();

                // Act
                await proxy.ReturnsTaskThatIsVoid();

                // Assert
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
            }

            [TestMethod]
            public async Task When_Method_Returns_A_Task_Result_Of_A_Primitive_Type()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsTaskResultThatIsAnIntWithValue25), withValue: Task.FromResult(13))
                   .Build();

                // Act
                await proxy.ReturnsTaskResultThatIsAnIntWithValue25();

                // Assert
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
            }

            [TestMethod]
            public async Task When_Method_Returns_A_Generic_Task_Result_Of_A_Primitive_Type()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsGenericTaskResultThatIsAnIntWithValue10), withValue: Task.FromResult(17))
                    .Build();

                // Act
                var result = await proxy.ReturnsGenericTaskResultThatIsAnIntWithValue10();

                // Assert
                result.Should().Be(17, because: "the method was stubbed with the value 17. ");

                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
            }

            [TestMethod]
            public void When_Method_Is_Stubbed_To_Prevent_Exception_Being_Thrown()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsTaskButThrowsAnExceptionInstead), Task.CompletedTask)
                    .Build();

                // Act
                var task = proxy.ReturnsTaskButThrowsAnExceptionInstead();

                // Assert
                task.Should().NotBeNull(because: "the exception was not thrown because we were stubbed. ");
            }

            [TestMethod]
            public async Task When_Async_Method_Returns_A_Void_Task()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.AsyncReturnsVoidTask), withValue: Task.CompletedTask)
                    .Build();

                // Act
                await proxy.AsyncReturnsVoidTask();

                // Assert
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");
            }

            [TestMethod]
            public async Task When_Async_Method_Returns_A_Generic_Task_Of_Objects()
            {
                // Arrange
                var proxy = _builder.InterceptAndStub(theMethodCalled: nameof(IAfterExecutionMethodSignatures.AsyncReturnsGenericTaskResult), withValue: Task.FromResult(new Product[1] {
                new Product()
                {
                    Name = "MockedName1",
                    Description = "MockedDescription1"
                }
            } as IEnumerable<Product>))
               .Build();

                // Act
                var result = await proxy.AsyncReturnsGenericTaskResult();

                // Assert
                _originalImplementation.Message.Should().Be(null, because: "the method was stubbed and not executed. ");

                var products = result as IEnumerable<Product>;
                products.Should().NotBeNull(because: "we have stubbed a single product. ");

                products.Count().Should().Be(1, because: "the stubbed to return one product ");
                products.First().Name.Should().Be("MockedName1", because: "that is the product name ");
                products.First().Description.Should().Be("MockedDescription1", because: "that is the product name ");
            }
        }

        [TestClass]
        public class Has_No_Effect_On_Behavior_If_Not_Stubbed : AfterExecutionOfTestBase
        {
            [TestMethod]
            public void When_Method_Is_Void()
            {
                // Arrange, Act
                _originalImplementation.IsVoid();

                // Assert
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.IsVoid)}", because: "the method should have fully completed without any callbacks. ");
            }

            [TestMethod]
            public void When_Method_Returns_A_Value()
            {
                // Arrange, Act
                _originalImplementation.ReturnsIntWithValue10();

                // Assert
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10)}", because: "the method should have fully completed without any callbacks. ");
            }

            [TestMethod]
            public async Task When_Method_Returns_A_Void_Task()
            {
                // Arrange, Act
                await _originalImplementation.ReturnsTaskThatIsVoid();

                // Assert
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.ReturnsTaskThatIsVoid)}", because: "the method should have fully completed without any callbacks. ");
            }

            [TestMethod]
            public async Task When_Method_Returns_Generic_Task_Of_Primitive_Type()
            {
                // Arrange, Act
                var result = await _originalImplementation.ReturnsGenericTaskResultThatIsAnIntWithValue10();

                // Assert
                result.Should().Be(10, because: "the task should have completed by now. ");
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.ReturnsGenericTaskResultThatIsAnIntWithValue10)}", because: "the method should have fully completed without any callbacks. ");
            }


            [TestMethod]
            [ExpectedException(typeof(AggregateException))]
            public void When_Method_Returns_A_Task_But_Throws_An_Exception()
            {
                // Arrange, Act, Assert
                _originalImplementation.ReturnsTaskButThrowsAnExceptionInstead().Wait();
            }

            [TestMethod]
            public async Task When_Async_Method_Returns_Void_Task()
            {
                // Arrange, Act
                await _originalImplementation.AsyncReturnsVoidTask();

                // Assert
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.AsyncReturnsVoidTask)}", because: "the method should have fully completed without any callbacks. ");
            }

            [TestMethod]
            public async Task When_Async_Method_Returns_Generic_Task_Of_Objects()
            {
                // Arrange, Act
                var result = await _originalImplementation.AsyncReturnsGenericTaskResult();

                // Assert
                result.Count().Should().Be(2, because: "that is how many products are returned in the real method. ");

                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.AsyncReturnsGenericTaskResult)}", because: "the method should have fully completed without any callbacks. ");
            }
        }
    }
}
