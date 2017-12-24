using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when an invalid email is used as a recipient email. The email can either be 
    /// in an incorrect format or the same email can already be attached to the package.
    /// </summary>
    [Serializable]
    public class InvalidEmailException : BaseException
    {
        public InvalidEmailException(String message)
            : base(message)
        {
            ;
        }
    }

}
