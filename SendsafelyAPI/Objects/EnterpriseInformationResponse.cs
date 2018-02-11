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
        private bool _allowUndisclosedRecipients;
        private bool _outlookBeta;
        private bool _messageEncryption;

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

        [JsonProperty(PropertyName = "allowUndisclosedRecipients")]
        public bool AllowUndisclosedRecipients
        {
            get { return _allowUndisclosedRecipients; }
            set { _allowUndisclosedRecipients = value; }
        }

        [JsonProperty(PropertyName = "outlookBeta")]
        public bool OutlookBeta
        {
            get { return _outlookBeta; }
            set { _outlookBeta = value; }
        }

        [JsonProperty(PropertyName = "messageEncryption")]
        public bool MessageEncryption
        {
            get { return _messageEncryption; }
            set { _messageEncryption = value; }
        }
    }
}
