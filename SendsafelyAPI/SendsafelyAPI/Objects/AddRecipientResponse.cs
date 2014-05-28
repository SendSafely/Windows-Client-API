using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddRecipientResponse
    {
        private APIResponse _response;
        private String _message;
        private String _recipientId;
        private bool _approvalRequired;
        private List<String> _approvers;
        private List<PhoneNumber> _phonenumbers;

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

        [JsonProperty(PropertyName = "recipientId")]
        public String RecipientId
        {
            get { return _recipientId; }
            set { _recipientId = value; }
        }

        [JsonProperty(PropertyName = "approvalRequired")]
        public bool ApprovalRequired
        {
            get { return _approvalRequired; }
            set { _approvalRequired = value; }
        }

        [JsonProperty(PropertyName = "approvers")]
        public List<String> Approvers
        {
            get { return _approvers; }
            set { _approvers = value; }
        }

        [JsonProperty(PropertyName = "phonenumbers")]
        public List<PhoneNumber> Phonenumbers
        {
            get { return _phonenumbers; }
            set { _phonenumbers = value; }
        }
    }
}
