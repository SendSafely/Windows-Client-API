using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class EnterpriseInformationResponse
    {

        private String _host;
        private String _systemName;

        [JsonProperty(PropertyName = "host")]
        public String Host
        {
            get { return _host; }
            set { _host = value; }
        }

        [JsonProperty(PropertyName = "systemName")]
        public String SystemName
        {
            get { return _systemName; }
            set { _systemName = value; }
        }

    }
}
