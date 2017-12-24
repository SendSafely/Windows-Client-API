using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class UpdatePackageDescriptorRequest
    {
        private String label;

        [JsonProperty(PropertyName = "label")]
        public String Label
        {
            get { return label; }
            set { label = value; }
        }

    }
}
