using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class AfterExecutionOfSyncTests
    {
        [TestClass]
        public class Interception_Is_Invoked : AfterExecutionOfTestBase
        {
            [TestMethod]
            public void When_A_Method_Is_Void()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.IsVoid), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                proxy.IsVoid();

                // Assert
                AssertReturnValueIsVoid(nameof(IAfterExecutionMethodSignatures.IsVoid), inResult: store);
            }

            [TestMethod]
            public void When_A_Method_Has_No_Parameters()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.HasNoParameters), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                proxy.HasNoParameters();

                // Assert
                store.Args.Should().NotBeNull(because: "when no parameters are provided, the Interceptor will return an empty collection. ");
                store.Parameters.Count.Should().Be(0, because: "there are no parameters for this method. ");
            }

            [TestMethod]
            public void When_A_Method_Has_One_Parameter()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.HasOneParameter), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                proxy.HasOneParameter(theInt: 32);

                // Assert
                store.Args.Should().NotBeNull(because: "the arguments passed to the method should always be returned. ");
                store.Args.Length.Should().Be(1, because: "the method call has one argument. ");

                store.Parameters.Count.Should().Be(1, because: "there are no parameters for this method. ");
                var theIntParameter = (int)store.Parameters["theInt"];
                theIntParameter.Should().Be(32, because: "that is the value passed in. ");
            }

            [TestMethod]
            public void When_A_Method_Has_Two_Parameters()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.HasTwoParameters), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                proxy.HasTwoParameters(theString: "woo", theInt: 33);

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
            public void When_A_Method_Returns_An_Integer()
            {
                // Arrange
                var store = default(IAfterExecutionResult);

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10), andCallbackWith: result =>
                {
                    store = result;
                })
                .Build();

                // Act
                proxy.ReturnsIntWithValue10();

                // Assert
                AssertReturnValue(nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10), isValue: 10, inResult: store);
            }

            [TestMethod]
            public void Two_Times_When_A_Method_Has_Two_Interceptions()
            {
                // Arrange
                var callback1 = false;
                var callback2 = false;

                var proxy = _builder
                    .InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10), andCallbackWith: result =>
                    {
                        callback1 = true;
                    })
                    .InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10), andCallbackWith: result =>
                    {
                        callback2 = true;
                    })
                    .Build();

                // Act
                proxy.ReturnsIntWithValue10();

                // Assert
                callback1.Should().BeTrue(because: "the first OnAfter callout will be invoked. ");
                callback2.Should().BeTrue(because: "the second OnAfter callout will be invoked. ");
            }

            [TestMethod]
            public void On_Intercepted_Methods_Only()
            {
                // Arrange
                var methodAsNoParametersCount = 0;
                var methodHasOneParametercount = 0;

                var proxy = _builder.InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.HasNoParameters), andCallbackWith: result =>
                {
                    methodAsNoParametersCount++;
                })
                .InterceptAfterExecutionOf(theMethodCalled: nameof(IAfterExecutionMethodSignatures.HasOneParameter), andCallbackWith: result =>
                {
                    methodHasOneParametercount++;
                }).Build();

                // Act 1
                proxy.HasNoParameters();

                // Assert 1
                methodAsNoParametersCount.Should().Be(1, because: "this was the method that was explicitly invoked. ");
                methodHasOneParametercount.Should().Be(0, because: "this method was not invoked. ");

                // Act 2
                proxy.HasOneParameter(10);

                // Assert 2
                methodAsNoParametersCount.Should().Be(1, because: "this method was not invoked this time. ");
                methodHasOneParametercount.Should().Be(1, because: "this method was invoked this time. ");
            }
        }

        [TestClass]
        public class Has_No_Effect_On_Behavior_If_Not_Configured : AfterExecutionOfTestBase
        {
            [TestMethod]
            public void When_A_Method_Is_Void()
            {
                // Arrange, Act
                _originalImplementation.IsVoid();

                // Assert
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.IsVoid)}", because: "the method should have fully completed without any callbacks. ");
            }

            [TestMethod]
            public void When_A_Method_Returns_An_Integer()
            {
                // Arrange, Act
                _originalImplementation.ReturnsIntWithValue10().Should().Be(10, because: "original behavior should be retained");

                // Assert
                _originalImplementation.Message.Should().Be($"Invoked: {nameof(IAfterExecutionMethodSignatures.ReturnsIntWithValue10)}", because: "the method should have fully completed without any callbacks. ");
            }
        }
    }
}
