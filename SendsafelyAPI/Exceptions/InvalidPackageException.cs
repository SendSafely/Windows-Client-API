using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when an invalid package ID is used. Will be thrown if the package ID is null, non-existent or for some reason inaccessible.
    /// </summary>
    [Serializable]
    public class InvalidPackageException : BaseException
    {
        public InvalidPackageException(String message)
            : base(message)
        {
            ;
        }
    }

}
