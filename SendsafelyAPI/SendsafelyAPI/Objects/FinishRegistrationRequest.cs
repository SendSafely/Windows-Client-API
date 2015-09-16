using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class FinishRegistrationRequest
    {
        [JsonProperty(PropertyName = "email")]
        public String Email { get; set; }

        [JsonProperty(PropertyName = "pinCode")]
        public String PinCode { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public String FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public String LastName { get; set; }

        [JsonProperty(PropertyName = "password")]
        public String Password { get; set; }

        [JsonProperty(PropertyName = "question")]
        public String Question { get; set; }

        [JsonProperty(PropertyName = "answer")]
        public String Answer { get; set; }

        [JsonProperty(PropertyName = "keyDescription")]
        public String KeyDescription { get; set; }
    }
}
