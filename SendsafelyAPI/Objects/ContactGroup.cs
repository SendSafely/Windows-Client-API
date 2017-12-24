using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ContactGroup
    {
        private String contactGroupId;
        private String contactGroupName;
        private Boolean contactGroupIsOrganizationGroup;
        private List<ContactGroupMember> users;

        [JsonProperty(PropertyName = "users")]
        public List<ContactGroupMember> Users
        {
            get { return users; }
            set { users = value; }
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

        [JsonProperty(PropertyName = "contactGroupIsOrganizationGroup")]
        public Boolean ContactGroupIsOrganizationGroup
        {
            get { return contactGroupIsOrganizationGroup; }
            set { contactGroupIsOrganizationGroup = value; }
        }

    }
}
