using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when we failed to create and add a public key
    /// </summary>
    [Serializable]
    public class GettingKeycodeFailedException : BaseException
    {
        public GettingKeycodeFailedException(String message)
            : base(message)
        {
            ;
        }
    }

}
