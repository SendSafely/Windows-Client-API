using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class PackageDTO
    {
        private String _packageId;
        private String _packageCode;
        private String _serverSecret;
        private APIResponse _response;
        private String _message;
        private bool _needsApprover;
        private List<String> _recipients;
        private List<String> _files;
        private List<String> _approvers;
        private int _life;

        [JsonProperty(PropertyName = "response")]
        internal APIResponse Response
        {
            get { return _response; }
            set { _response = value; }
        }

        [JsonProperty(PropertyName = "packageId")]
        internal String PackageID
        {
            get { return _packageId; }
            set { _packageId = value; }
        }

        [JsonProperty(PropertyName = "packageCode")]
        internal String PackageCode
        {
            get { return _packageCode; }
            set { _packageCode = value; }
        }

        [JsonProperty(PropertyName = "serverSecret")]
        internal String ServerSecret
        {
            get { return _serverSecret; }
            set { _serverSecret = value; }
        }

        [JsonProperty(PropertyName = "message")]
        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }

        [JsonProperty(PropertyName = "needsApproval")]
        public bool NeedsApprover
        {
            get { return _needsApprover; }
            set { _needsApprover = value; }
        }

        [JsonProperty(PropertyName = "recipients")]
        internal List<String> Recipients
        {
            get { return _recipients; }
            set { _recipients = value; }
        }

        [JsonProperty(PropertyName = "filenames")]
        public List<String> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        [JsonProperty(PropertyName = "approverList")]
        public List<String> Approvers
        {
            get { return _approvers; }
            set { _approvers = value; }
        }

        [JsonProperty(PropertyName = "life")]
        public int Life
        {
            get { return _life; }
            set { _life = value; }
        }
    }
}
