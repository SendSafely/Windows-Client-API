using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when the specified user's password does not match the servers demand on password complexity.
    /// </summary>
    [Serializable]
    public class InsufficientPasswordComplexityException : BaseException
    {
        public InsufficientPasswordComplexityException(String message)
            : base(message)
        {
            ;
        }
    }

}
