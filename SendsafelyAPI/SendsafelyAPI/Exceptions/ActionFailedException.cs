using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// This exception is thrown when the server returned an unexpected response message. The reason can be found in the 
    /// Reason variable. The exception message will contain a longer explanation to the error.
    /// </summary>
    [Serializable]
    public class ActionFailedException : BaseException
    {
        private String _reason;

        public ActionFailedException(String reason, String message)
            : base(message)
        {
            this._reason = reason;
        }

        public String Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }
    }
}
