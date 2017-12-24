using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GetUserGroupsResponse : StandardResponse
    {
        private List<ContactGroup> contactGroups;

        [JsonProperty(PropertyName = "contactGroups")]
        public List<ContactGroup> ContactGroups
        {
            get { return contactGroups; }
            set { contactGroups = value; }
        }
    }
}
