using System;
using System.Collections.Generic;
using System.Text;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    public class CustomTaskException : System.Exception
    {
        public CustomTaskException() : base($"This is a custom exception")
        {

        }
    }
}
