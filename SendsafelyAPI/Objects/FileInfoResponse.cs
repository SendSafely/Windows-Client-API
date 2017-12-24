using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class FileInformationResponse : StandardResponse
    {
        private FileInformation file;

        [JsonProperty(PropertyName = "file")]
        internal FileInformation File
        {
            get { return file; }
            set { file = value; }
        }
    }
}
