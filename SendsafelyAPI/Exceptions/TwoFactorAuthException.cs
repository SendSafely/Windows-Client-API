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
        private String _twoFaType;

        public TwoFactorAuthException(String validationToken, String twoFaType)
        {
            this._validationToken = validationToken;
            this._twoFaType = twoFaType;
        }

        public String ValidationToken
        {
            get { return _validationToken; }
            set { _validationToken = value; }
        }
        
        public String TwoFaType
        {
            get { return _twoFaType; }
            set { _twoFaType = value; }
        }
    }
}
