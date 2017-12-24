using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class UploadFileRequest
    {
        [JsonProperty(PropertyName = "uploadType")]
        public String UploadType { get; set; }

        [JsonProperty(PropertyName = "filePart")]
        public int FilePart { get; set; }
    }
}
