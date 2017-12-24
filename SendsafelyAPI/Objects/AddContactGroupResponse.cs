using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddContactGroupResponse : StandardResponse
    {
        private String contactGroupId;
        private String contactGroupName;
        private List<String> contactGroupUserEmails;

        [JsonProperty(PropertyName = "contactGroupUserEmails")]
        public List<String> ContactGroupUserEmails
        {
            get { return contactGroupUserEmails; }
            set { contactGroupUserEmails = value; }
        }

        [JsonProperty(PropertyName = "contactGroupId")]
        public String ContactGroupId
        {
            get { return contactGroupId; }
            set { contactGroupId = value; }
        }

        [JsonProperty(PropertyName = "contactGroupName")]
        public String ContactGroupName
        {
            get { return contactGroupName; }
            set { contactGroupName = value; }
        }

    }
}
