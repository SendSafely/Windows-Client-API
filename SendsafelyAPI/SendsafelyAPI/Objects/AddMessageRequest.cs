using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddMessageRequest
    {

        [JsonProperty(PropertyName = "uploadType")]
        public String UploadType { get; set; }

        [JsonProperty(PropertyName = "message")]
        public String Message { get; set; }
    }
}
