using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GenerateAPIKeyResponse
    {
        private APIResponse _response;
        private String _message;
        
        private String _email;
	    private String _apiKey;
	    private String _apiSecret;
        
        [JsonProperty(PropertyName = "response")]
        internal APIResponse Response
        {
            get { return _response; }
            set { _response = value; }
        }

        [JsonProperty(PropertyName = "message")]
        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }

        [JsonProperty(PropertyName = "email")]
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [JsonProperty(PropertyName = "apiKey")]
        public String APIKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        [JsonProperty(PropertyName = "apiSecret")]
        public String APISecret
        {
            get { return _apiSecret; }
            set { _apiSecret = value; }
        }
    }
}
