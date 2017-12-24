using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class CreateDirectoryRequest
    {
        [JsonProperty(PropertyName = "directoryName")]
        public String DirectoryName { get; set; }

    }
}
