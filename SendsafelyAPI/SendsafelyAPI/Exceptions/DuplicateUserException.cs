using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when a Google Auth user already exists as a native user
    /// </summary>
    [Serializable]
    public class DuplicateUserException : BaseException
    {
        public DuplicateUserException(String message)
            : base(message)
        {
            ;
        }
    }

}
