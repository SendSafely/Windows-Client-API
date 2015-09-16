using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when an invalid package secure link is used. Will be thrown if the link is null, incorrect or for some reason inaccessible.
    /// </summary>
    [Serializable]
    public class InvalidLinkException : BaseException
    {
        public InvalidLinkException(String message)
            : base(message)
        {
            ;
        }
    }

}
