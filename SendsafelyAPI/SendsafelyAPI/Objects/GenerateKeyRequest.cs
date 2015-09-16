using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GenerateKeyRequest
    {

        [JsonProperty(PropertyName = "email")]
        public String Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public String Password { get; set; }

        [JsonProperty(PropertyName = "keyDescription")]
        public String KeyDescription { get; set; }

        [JsonProperty(PropertyName = "smsCode")]
        public String SMSCode { get; set; }
    }
}
