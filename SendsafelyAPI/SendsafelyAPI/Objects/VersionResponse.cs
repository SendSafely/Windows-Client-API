using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class VersionResponse
    {
        private Version _version;
        private APIResponse _response;
        private String _message;

        [JsonProperty(PropertyName = "version")]
        public Version Version
        {
            get { return _version; }
            set { _version = value; }
        }

        [JsonProperty(PropertyName = "response")]
        public APIResponse Response
        {
            get { return _response; }
            set { _response = value; }
        }

        [JsonProperty(PropertyName = "message")]
        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
