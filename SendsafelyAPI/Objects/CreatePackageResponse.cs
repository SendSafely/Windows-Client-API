using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class CreatePackageResponse
    {
        private APIResponse _response;
        private String _packageId;
        private String _message;
        private String _serverSecret;
        private String _packageCode;
        private String _rootDirectoryId;

        [JsonProperty(PropertyName = "response")]
        internal APIResponse Response
        {
            get { return _response; }
            set { _response = value; }
        }

        [JsonProperty(PropertyName = "packageId")]
        public String PackageId
        {
            get { return _packageId; }
            set { _packageId = value; }
        }

        [JsonProperty(PropertyName = "serverSecret")]
        public String ServerSecret
        {
            get { return _serverSecret; }
            set { _serverSecret = value; }
        }

        [JsonProperty(PropertyName = "packageCode")]
        public String PackageCode
        {
            get { return _packageCode; }
            set { _packageCode = value; }
        }

        [JsonProperty(PropertyName = "message")]
        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }

        [JsonProperty(PropertyName = "rootDirectoryId")]
        public String RootDirectoryId
        {
            get { return _rootDirectoryId; }
            set { _rootDirectoryId = value; }
        }
    }

    
}
