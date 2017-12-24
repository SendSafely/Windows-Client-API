using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddGroupResponse : StandardResponse
    {
        private String userId;
        private String userEmail;

        [JsonProperty(PropertyName = "userId")]
        public String UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        [JsonProperty(PropertyName = "userEmail")]
        public String UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }
    }
}
