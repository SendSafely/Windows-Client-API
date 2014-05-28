using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when the package can for some reason not be finalized. 
    /// The exception contains a list of errors that prevented the finalization.
    /// </summary>
    [Serializable]
    public class PackageFinalizationException : BaseException
    {
        private List<String> _errors;

        public PackageFinalizationException(List<String> errors)
        {
            this._errors = errors;
        }

        public List<String> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }
    }
}
