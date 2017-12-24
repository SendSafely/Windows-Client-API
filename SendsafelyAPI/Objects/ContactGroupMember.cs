using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ContactGroupMember
    {
        private String userEmail;
        private String userId;
        
        [JsonProperty(PropertyName = "userEmail")]
        public String UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }

        [JsonProperty(PropertyName = "userId")]
        public String UserId
        {
            get { return userId; }
            set { userId = value; }
        }
    }
}
