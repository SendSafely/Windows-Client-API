using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class PackageInformationResponse
    {
        private String _packageId;
        private String _packageCode;
        private String _serverSecret;
        private APIResponse _response;
        private String _message;
        private bool _needsApprover;
        private String state;
        private List<Recipient> _recipients;
        private List<File> _files;
        private List<String> _approvers;
        private int _life;
        private DateTime _packageTimestamp;
        private String _packageSender;
        private String _rootDirectoryId;
        private String _label;
        private bool _isVDR;
        private List<ContactGroup> contactGroups;

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
        internal List<Recipient> Recipients
        {
            get { return _recipients; }
            set { _recipients = value; }
        }

        [JsonProperty(PropertyName = "files")]
        public List<File> Files
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

        [JsonProperty(PropertyName = "packageTimestamp")]
        public DateTime PackageTimestamp
        {
            get { return _packageTimestamp; }
            set { _packageTimestamp = value; }
        }

        [JsonProperty(PropertyName = "packageSender")]
        public String PackageSender
        {
            get { return _packageSender; }
            set { _packageSender = value; }
        }

        [JsonProperty(PropertyName = "rootDirectoryId")]
        public String RootDirectoryId
        {
            get { return _rootDirectoryId; }
            set { _rootDirectoryId = value; }
        }

        [JsonProperty(PropertyName = "label")]
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        [JsonProperty(PropertyName = "isVDR")]
        public Boolean IsVDR
        {
            get { return _isVDR; }
            set { _isVDR = value; }
        }

        /// <summary>
        /// A list of contact groups
        /// </summary>
        [JsonProperty(PropertyName = "contactGroups")]
        public List<ContactGroup> ContactGroups
        {
            get { return contactGroups; }
            set { contactGroups = value; }
        }

        /// <summary>
        /// a string of state
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public String State
        {
            get { return state; }
            set { state = value; }
        }

    }
}
