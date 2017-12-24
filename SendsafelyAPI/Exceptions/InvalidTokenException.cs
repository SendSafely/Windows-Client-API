using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when the user supplies an invalid token or pin code to the server.
    /// </summary>
    [Serializable]
    public class InvalidTokenException : BaseException
    {
        public InvalidTokenException()
            : base()
        { }

        public InvalidTokenException(string message)
            : base(message)
        { }
    }
}
