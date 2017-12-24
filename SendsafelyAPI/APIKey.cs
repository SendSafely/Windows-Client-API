using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely
{
    public class APICredential
    {

        private String apiKey;
        private String apiSecret;

        public String APIKey
        {
            get { return apiKey; }
            set { apiKey = value; }
        }

        public String APISecret
        {
            get { return apiSecret; }
            set { apiSecret = value; }
        }
    }
}
