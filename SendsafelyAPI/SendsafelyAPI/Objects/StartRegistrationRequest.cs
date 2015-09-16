using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class StartRegistrationRequest
    {
        private String _email;
        private Boolean _sendPin;

        [JsonProperty(PropertyName = "email")]
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [JsonProperty(PropertyName = "sendPin")]
        public Boolean SendPin
        {
            get { return _sendPin; }
            set { _sendPin = value; }
        }
    }
}
