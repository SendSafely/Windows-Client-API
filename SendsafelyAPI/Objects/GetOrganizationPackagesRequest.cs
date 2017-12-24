using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GetOrganizationPackagesRequest
    {

        [JsonProperty(PropertyName = "fromDate")]
        public String FromDate { get; set; }

        [JsonProperty(PropertyName = "toDate")]
        public String ToDate { get; set; }

        [JsonProperty(PropertyName = "sender")]
        public String Sender { get; set; }

        [JsonProperty(PropertyName = "status")]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "recipient")]
        public String Recipient { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public String Filename { get; set; }
    }
}
