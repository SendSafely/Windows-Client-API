using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class CreatePackageRequest
    {
        [JsonProperty(PropertyName = "packageUserEmail")]
        public String PackageUserEmail { get; set; }

        [JsonProperty(PropertyName = "vdr")]
        public Boolean Vdr { get; set; }
    }
}
