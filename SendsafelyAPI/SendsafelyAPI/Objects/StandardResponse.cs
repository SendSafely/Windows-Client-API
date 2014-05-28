using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class StandardResponse
    {
        private APIResponse _response;
        private String _message;

        [JsonProperty(PropertyName = "response")]
        internal APIResponse Response
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
