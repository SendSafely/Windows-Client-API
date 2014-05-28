using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class FinalizePackageResponse
    {
        private APIResponse _response;
        private String _message;
        private List<String> _errors;
        private List<String> _approvers;

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

        [JsonProperty(PropertyName = "errors")]
        public List<String> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        [JsonProperty(PropertyName = "approvers")]
        public List<String> Approvers
        {
            get { return _approvers; }
            set { _approvers = value; }
        }
    }
}
