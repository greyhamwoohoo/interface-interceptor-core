using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.Rules
{
    [TestClass]
    public class StubExecutionRuleTests
    {
        [TestMethod]
        public void When_Fixed_Value_Is_Provided()
        {
            // Arrange, Act
            var stubExecutionRule = new StubExecutionRule("theMethodName", 10);

            // Assert
            stubExecutionRule.IsFixedValue.Should().BeTrue(because: "an explicit value was provided. ");
            stubExecutionRule.IsDynamicValue.Should().BeFalse(because: "an explicit value was provided. ");
            stubExecutionRule.IsVoid.Should().BeFalse(because: "either a value or dynamic callback was provided. ");
            stubExecutionRule.MethodName.Should().Be("theMethodName", because: "the rule was configured for that method name. ");

            stubExecutionRule.DynamicValueProvider.Should().BeNull(because: "no callback was provided. ");
        }

        [TestMethod]
        public void When_Callback_To_Return_Dynamic_Value_Is_Provided()
        {
            // Arrange, Act
            var stubExecutionRule = new StubExecutionRule("theMethodName", withValueProvider: callContext => { return 10; });

            // Assert
            stubExecutionRule.IsFixedValue.Should().BeFalse(because: "a callback was provided. ");
            stubExecutionRule.IsDynamicValue.Should().BeTrue(because: "a callback was provided for an explicit callback. ");
            stubExecutionRule.IsVoid.Should().BeFalse(because: "either a value or dynamic callback was provided. ");
            stubExecutionRule.MethodName.Should().Be("theMethodName", because: "the rule was configured for that method name. ");

            stubExecutionRule.DynamicValueProvider.Should().NotBeNull(because: "a callback was provided. ");
        }

        [TestMethod]
        public void When_No_Callback_And_No_Fixed_Value_Provided()
        {
            // Arrange, Act
            var stubExecutionRule = new StubExecutionRule("theMethodName");

            // Assert
            stubExecutionRule.IsFixedValue.Should().BeFalse(because: "the method is value. ");
            stubExecutionRule.IsDynamicValue.Should().BeFalse(because: "the method is value. ");
            stubExecutionRule.IsVoid.Should().BeTrue(because: "no callback handler and no fixed value was provided. ");
            stubExecutionRule.MethodName.Should().Be("theMethodName", because: "the rule was configured for that method name. ");
        }
    }
}
