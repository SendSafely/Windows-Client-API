using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GetPublicKeysResponse
    {
        private APIResponse _response;
        private String _message;

        private List<PublicKeyRaw> _publicKeys;

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

        [JsonProperty(PropertyName = "publicKeys")]
        public List<PublicKeyRaw> PublicKeys
        {
            get { return _publicKeys; }
            set { _publicKeys = value; }
        }
    }
}
