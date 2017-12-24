using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class UserInformationResponse
    {

        private String _id;
        private String _email;
        private String _clientKey;
        private String _firstName;
        private String _lastName;
        private bool _publicKey;
        private int _packageLife;
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

        [JsonProperty(PropertyName = "id")]
        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonProperty(PropertyName = "email")]
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [JsonProperty(PropertyName = "clientKey")]
        public String ClientKey
        {
            get { return _clientKey; }
            set { _clientKey = value; }
        }

        [JsonProperty(PropertyName = "firstName")]
        public String FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        [JsonProperty(PropertyName = "lastName")]
        public String LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        [JsonProperty(PropertyName = "publicKey")]
        public bool AllowPublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        [JsonProperty(PropertyName = "packageLife")]
        public int PackageLife
        {
            get { return _packageLife; }
            set { _packageLife = value; }
        }
    }
}
