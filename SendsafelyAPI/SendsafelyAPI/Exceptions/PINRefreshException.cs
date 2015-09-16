using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when an invalid PIN has been used 5 or more times. When that happens the server will send a new pin to the users email that should be used instead.
    /// </summary>
    [Serializable]
    public class PINRefreshException : BaseException
    {

        public PINRefreshException()
            : base()
        { }

        public PINRefreshException(string message)
            : base(message)
        { }
    }
}
