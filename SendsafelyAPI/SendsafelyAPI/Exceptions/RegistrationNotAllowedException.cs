using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when a user is not allowed to register an account. This will likely happen because the users organization does not allow it.
    /// </summary>
    [Serializable]
    public class RegistrationNotAllowedException : BaseException
    {
        public RegistrationNotAllowedException()
            : base()
        { }

        public RegistrationNotAllowedException(string message)
            : base(message)
        { }
    }
}
