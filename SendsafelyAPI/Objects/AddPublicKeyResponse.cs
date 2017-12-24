using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddPublicKeyResponse
    {
        private APIResponse _response;
        private String _message;
        private String _id;
	    private String _description;
	    private String _dateStr;

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

        [JsonProperty(PropertyName = "id")]
        public String ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonProperty(PropertyName = "description")]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [JsonProperty(PropertyName = "dateStr")]
        public String DateStr
        {
            get { return _dateStr; }
            set { _dateStr = value; }
        }
    }
}
