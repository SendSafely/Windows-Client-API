using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class CreateFileIdRequest
    {

        [JsonProperty(PropertyName = "uploadType")]
        public String UploadType { get; set; }

        [JsonProperty(PropertyName = "parts")]
        public int Parts { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public String Filename { get; set; }

        [JsonProperty(PropertyName = "filesize")]
        public long Filesize { get; set; }

        [JsonProperty(PropertyName = "directoryId")]
        public String DirectoryId { get; set; }
    }
}
