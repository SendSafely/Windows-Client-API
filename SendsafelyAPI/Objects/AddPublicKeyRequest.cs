using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddPublicKeyRequest
    {
        [JsonProperty(PropertyName = "publicKey")]
        public String PublicKey { get; set; }

        [JsonProperty(PropertyName = "description")]
        public String Description { get; set; }
    }
}
