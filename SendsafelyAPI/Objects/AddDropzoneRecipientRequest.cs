using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddDropzoneRecipientRequest
    {
        private String _userEmail;

        [JsonProperty(PropertyName = "userEmail")]
        public String UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }
    }
}
