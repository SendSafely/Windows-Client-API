using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddRecipientResponse : StandardResponse
    {
        private String _recipientId;
        private String _email;
        private bool _approvalRequired;
        private List<String> _approvers;
        private List<PhoneNumber> _phonenumbers;
        private String _roleName;


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

        [JsonProperty(PropertyName = "email")]
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [JsonProperty(PropertyName = "roleName")]
        public String RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }
    }
}
