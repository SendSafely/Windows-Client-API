using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when a keycode is needed but not found by the API. If the keycode is not present it must be passed into the application by the user.
    /// </summary>
    [Serializable]
    public class MissingKeyCodeException : BaseException
    {
        public MissingKeyCodeException()
            : base()
        { }

        public MissingKeyCodeException(string message)
            : base(message)
        { }
    }
}
