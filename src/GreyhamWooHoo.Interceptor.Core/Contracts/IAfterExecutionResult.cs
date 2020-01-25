namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    /// <summary>
    /// Passed to the callback after the method has been invoked but before the return value is passed to the caller.
    /// </summary>
    public interface IAfterExecutionResult
    {
        IAfterExecutionRule Rule { get; }
        bool HasReturnValue { get; }
        object ReturnValue { get; }
    }
}
