using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.Contracts.Generic;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ServicesToIntercept.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    /// <summary>
    /// A single method can have an OnBefore, Stub, and OnAfter handler. 
    /// </summary>
    [TestClass]
    public class BeforeStubAfterTests
    {
        private readonly BeforeStubAfterMethodSignatures _originalImplementation = new BeforeStubAfterMethodSignatures();

        private IInterceptorProxyBuilder<IBeforeStubAfterMethodSignatures> _builder;

        [TestInitialize]
        public void SetupReturnValueTests()
        {
            _builder = new InterceptorProxyBuilder<IBeforeStubAfterMethodSignatures>()
                .For(_originalImplementation);
        }

        [TestMethod]
        public void Assumptions()
        {
            _originalImplementation.Echo(12).Should().Be(12, because: "the method echos what is put in. ");
            _originalImplementation.Echo(22).Should().Be(22, because: "the method echos what is put in. ");
        }

        [TestMethod]
        public void Can_Provide_Before_Stub_And_After_Interceptions()
        {
            var beforeResult = 0;
            var afterResult = 0;

            var iut = _builder.InterceptBeforeExecutionOf(nameof(IBeforeStubAfterMethodSignatures.Echo), andCallBackWith: result =>
            {
                beforeResult = (int)result.Parameters["value"];
            })
            .InterceptAndStub(nameof(IBeforeStubAfterMethodSignatures.Echo), withValue: 27)
            .InterceptAfterExecutionOf(nameof(IBeforeStubAfterMethodSignatures.Echo), andCallbackWith: result =>
            {
                afterResult = (int) result.ReturnValue;
            })
            .Build();

            var methodResult = iut.Echo(25);

            beforeResult.Should().Be(25, because: "the callback should have been invoked and the value set. ");
            methodResult.Should().Be(27, because: "a stubbed value of 27 was explicitly returned. ");
            afterResult.Should().Be(27, because: "the stubbed value is the one used after the method is invoked. ");
        }
    }
}
