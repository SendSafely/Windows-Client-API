using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddRecipientRequest
    {
        private String _email;

        [JsonProperty(PropertyName = "email")]
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }
    }
}
