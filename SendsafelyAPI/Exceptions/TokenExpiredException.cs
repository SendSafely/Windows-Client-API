using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when the token or PIN has expired. When this happens, the client should reach out and ask for a new token.
    /// </summary>
    [Serializable]
    public class TokenExpiredException : BaseException
    {

        public TokenExpiredException()
            : base()
        { }

        public TokenExpiredException(string message)
            : base(message)
        { }
    }
}
