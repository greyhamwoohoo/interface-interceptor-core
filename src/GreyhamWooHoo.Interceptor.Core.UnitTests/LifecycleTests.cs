using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.UnitTests.LifecycleExecution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    /// <summary>
    /// A single method can have an OnBefore, Stub, and OnAfter handler. 
    /// </summary>
    [TestClass]
    public class LifecycleExecutionTests
    {
        private readonly LifecycleExecutionTestImplementation _originalImplementation = new LifecycleExecutionTestImplementation();

        private InterceptorProxyBuilder<ILifecycleExecutionTestInterface> _builder;

        [TestInitialize]
        public void SetupReturnValueTests()
        {
            _builder = new InterceptorProxyBuilder<ILifecycleExecutionTestInterface>()
                .For(_originalImplementation);
        }

        [TestMethod]
        public void EchoSanity()
        {
            _originalImplementation.Echo(12).Should().Be(12, because: "the method echos what is put in. ");
            _originalImplementation.Echo(22).Should().Be(22, because: "the method echos what is put in. ");
        }

        [TestMethod]
        public void All()
        {
            var beforeResult = 0;
            var afterResult = 0;

            var iut = _builder.InterceptBeforeExecutionOf(nameof(ILifecycleExecutionTestInterface.Echo), andCallBackWith: result =>
            {
                beforeResult = (int)result.Parameters["value"];
            })
            .InterceptAndStub(nameof(ILifecycleExecutionTestInterface.Echo), withValue: 27)
            .InterceptAfterExecutionOf(nameof(ILifecycleExecutionTestInterface.Echo), andCallbackWith: result =>
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
