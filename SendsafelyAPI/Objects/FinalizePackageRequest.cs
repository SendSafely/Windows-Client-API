using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class FinalizePackageRequest
    {
        private String _password;
        private String _checksum;
        private bool undisclosedRecipients; 

        [JsonProperty(PropertyName = "password")]
        public String Password { get; set; }

        [JsonProperty(PropertyName = "checksum")]
        public String Checksum { get; set; }

        [JsonProperty(PropertyName = "undisclosedRecipients")]
        public bool UndisclosedRecipients { get; set; }
    }
}
