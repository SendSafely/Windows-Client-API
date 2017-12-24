using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserDTO
    {
        private String userEmail;
        private String userId;

        [JsonProperty(PropertyName = "userEmail")]
        internal String UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }
        [JsonProperty(PropertyName = "userId")]
        internal String UserId
        {
            get { return userId; }
            set { userId = value; }
        }

    }
}
