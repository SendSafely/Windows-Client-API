using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FileResponse
    {
        private String fileId;
        private String fileName;
        private long fileSize;
        private String createdByEmail;
        private int parts;

        [JsonProperty(PropertyName = "fileId")]
        public String FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        [JsonProperty(PropertyName = "fileName")]
        public String FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        [JsonProperty(PropertyName = "fileSize")]
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        [JsonProperty(PropertyName = "createdByEmail")]
        public String CreatedByEmail
        {
            get { return createdByEmail; }
            set { createdByEmail = value; }
        }

        [JsonProperty(PropertyName = "parts")]
        public int Parts
        {
            get { return parts; }
            set { parts = value; }
        }
    }
}
