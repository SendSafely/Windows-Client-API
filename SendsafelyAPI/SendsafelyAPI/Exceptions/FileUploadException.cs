using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Generic upload exception which can be thrown when an exception is thrown during the upload or encryption phase.
    /// The most common reason for this exception is an interrupted internet connection.
    /// </summary>
    [Serializable]
    public class FileUploadException : BaseException
    {
        public FileUploadException()
            : base()
        { }

        public FileUploadException(string message)
            : base(message)
        { }
    }
}
