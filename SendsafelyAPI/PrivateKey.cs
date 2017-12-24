using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely
{
    public class PrivateKey
    {

        private String _privateKey;
        private String _publicKeyId;

        public String ArmoredKey
        {
            get { return _privateKey; }
            set { _privateKey = value; }
        }

        public String PublicKeyID
        {
            get { return _publicKeyId; }
            set { _publicKeyId = value; }
        }
    }
}
