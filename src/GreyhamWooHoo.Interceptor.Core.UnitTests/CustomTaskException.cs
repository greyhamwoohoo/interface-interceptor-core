namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    public class CustomTaskException : System.Exception
    {
        public CustomTaskException() : base($"This is a custom exception")
        {

        }
    }
}
