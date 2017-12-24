using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when the API fails to connect to the server.
    /// </summary>
    [Serializable]
    public class ServerUnavailableException : BaseException
    {
        public ServerUnavailableException()
            : base()
        { }

        public ServerUnavailableException(string message)
            : base(message)
        { }
    }
}
