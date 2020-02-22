using FluentAssertions;
using FluentAssertions.Execution;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Contracts.Generic;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class AfterTestBase
    {
        protected readonly AfterExecutionTestImplementation _originalImplementation = new AfterExecutionTestImplementation();

        protected IInterceptorProxyBuilder<IAfterExecutionTestInterface> _builder;

        [TestInitialize]
        public void SetupReturnValueTests()
        {
            _builder = new InterceptorProxyBuilder<IAfterExecutionTestInterface>()
                .For(_originalImplementation);
        }

        protected void AssertReturnValue(string forMethod, int isValue, IAfterExecutionResult inResult)
        {
            AssertReturnValue(forMethod, true, inResult);
            
            inResult.ReturnValue.Should().Be(isValue, because: "that is the hard coded value returned from the method. ");
        }

        protected void AssertReturnValueIsVoid(string forMethod, IAfterExecutionResult inResult)
        {
            AssertReturnValue(forMethod, false, inResult);
         
            inResult.ReturnValue.Should().BeNull(because: "by design, we ensure that a void method will return null; use HasReturnValue as a discriminator. ");
        }

        protected void AssertReturnValue(string forMethod, bool hasReturnValue, IAfterExecutionResult inResult)
        {
            _originalImplementation.Message.Should().Be($"Invoked: {forMethod}", because: "the method should have been invoked before the callback. ");

            inResult.Should().NotBeNull(because: "the callback should have been invoked with the return value result. ");
            using (var scope = new AssertionScope())
            {
                inResult.HasReturnValue.Should().Be(hasReturnValue, because: "a void method has no return value. ");
            }

            inResult.Rule.Should().NotBeNull(because: "the intercept rule should always be passed to the callback. ");
            inResult.Rule.MethodName.Should().Be(forMethod);
        }
    }
}
