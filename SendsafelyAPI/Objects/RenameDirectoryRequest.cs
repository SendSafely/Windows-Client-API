using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class RenameDirectoryRequest
    {
        private String _directoryName;

        [JsonProperty(PropertyName = "directoryName")]
        public String DirectoryName
        {
            get { return _directoryName; }
            set { _directoryName = value; }
        }
    }
}
