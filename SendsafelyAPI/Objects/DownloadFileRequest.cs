using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class DownloadFileRequest
    {
        [JsonProperty(PropertyName = "checksum")]
        public String Checksum { get; set; }

        [JsonProperty(PropertyName = "part")]
        public int Part { get; set; }

        [JsonProperty(PropertyName = "api")]
        public String Api { get; set; }

        [JsonProperty(PropertyName = "password")]
        public String Password { get; set; }
    }
}
