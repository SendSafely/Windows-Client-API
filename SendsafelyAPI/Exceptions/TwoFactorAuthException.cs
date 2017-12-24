using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when two factor authentication is required. The exception contains a ValidationToken parameter that must be used when validating the 2FA Code.
    /// </summary>
    [Serializable]
    public class TwoFactorAuthException : BaseException
    {

        private String _validationToken;

        public TwoFactorAuthException(String validationToken)
        {
            this._validationToken = validationToken;
        }

        public String ValidationToken
        {
            get { return _validationToken; }
            set { _validationToken = value; }
        }
    }
}
