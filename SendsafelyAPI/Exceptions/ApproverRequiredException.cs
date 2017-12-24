using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Will be thrown if a package that requires an approver is finalized without having one. 
    /// When this is thrown, an approver must be added as a recipient before the package can be finalized.
    /// </summary>
    [Serializable]
    public class ApproverRequiredException : BaseException
    {
        public ApproverRequiredException()
            : base()
        { }

        public ApproverRequiredException(string message)
            : base(message)
        { }
    }
}
