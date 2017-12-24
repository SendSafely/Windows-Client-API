using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when we failed to create and add a public key
    /// </summary>
    [Serializable]
    public class AddingPublicKeyFailedException : BaseException
    {
        public AddingPublicKeyFailedException(String message)
            : base(message)
        {
            ;
        }
    }

}
