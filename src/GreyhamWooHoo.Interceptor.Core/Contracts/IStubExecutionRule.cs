namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IStubExecutionRule : IExecutionRule
    {
        object Value { get; }
    }
}
