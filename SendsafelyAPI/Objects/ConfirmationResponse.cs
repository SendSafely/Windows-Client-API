using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class ConfirmationResponse
    {
        private String ipAddress;
        private DateTime timestamp;
        private FileResponse file;

        [JsonProperty(PropertyName = "ipAddress")]
        public String IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        [JsonProperty(PropertyName = "file")]
        public FileResponse File
        {
            get { return file; }
            set { file = value; }
        }
    }
}
