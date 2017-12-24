using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when we failed to revoke a public key
    /// </summary>
    [Serializable]
    public class RevokingKeyFailedException : BaseException
    {
        public RevokingKeyFailedException(String message)
            : base(message)
        {
            ;
        }
    }

}
