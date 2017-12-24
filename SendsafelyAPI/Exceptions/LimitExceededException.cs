using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when the account limitation is exceeded for the user. 
    /// The exception message will contain information about what limitation has been exceeded.
    /// </summary>
    [Serializable]
    public class LimitExceededException : BaseException
    {
        public LimitExceededException(String message)
            : base(message)
        {
            ;
        }
    }

}
