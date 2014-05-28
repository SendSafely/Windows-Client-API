using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Will be thrown when the API is not yet initialized.
    /// </summary>
    [Serializable]
    public class APINotInitializedException : BaseException
    {
        public APINotInitializedException()
            : base()
        { }

        public APINotInitializedException(string message)
            : base(message)
        { }
    }
}
