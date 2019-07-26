using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when an organization enforce two fa flag is true
    /// </summary>
    [Serializable]
    public class TwoFAEnforcedException : BaseException
    {
        public TwoFAEnforcedException(String message)
            : base(message)
        {
            ;
        }
    }

}
