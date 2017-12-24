using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddContactGroupRequest
    {
        private String _groupName;
        private String _isEnterpriseGroup;

        [JsonProperty(PropertyName = "groupName")]
        public String GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        [JsonProperty(PropertyName = "isEnterpriseGroup")]
        public String IsEnterpriseGroup
        {
            get { return _isEnterpriseGroup; }
            set { _isEnterpriseGroup = value; }
        }
    }
}
