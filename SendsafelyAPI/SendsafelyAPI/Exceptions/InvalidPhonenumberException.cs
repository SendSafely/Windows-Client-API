using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when the supplied phone number is in a format not recognized by the server.
    /// </summary>
    [Serializable]
    public class InvalidPhonenumberException : BaseException
    {
        public InvalidPhonenumberException(String message)
            : base(message)
        {
            
        }
    }
}
