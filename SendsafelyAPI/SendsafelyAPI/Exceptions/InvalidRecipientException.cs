using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when an attempt to update a recipient with an incorrect recipient ID is made.
    /// </summary>
    [Serializable]
    public class InvalidRecipientException : BaseException
    {
        public InvalidRecipientException(String message)
            : base(message)
        {
            
        }
    }
}
