namespace GreyhamWooHoo.Interceptor.Core.UnitTests.Exceptions
{
    public class CustomTaskException : System.Exception
    {
        public CustomTaskException() : base($"This is a custom exception")
        {

        }
    }
}
