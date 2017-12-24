using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when the public key does not contain a public key
    /// </summary>
    [Serializable]
    public class InvalidKeyException : BaseException
    {
        public InvalidKeyException(String message)
            : base(message)
        {
            ;
        }
    }

}
