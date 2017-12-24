using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class UpdateRecipientRequest
    {
        private String _phoneNumber;
        private String _countrycode;
        private String _roleName;

        [JsonProperty(PropertyName = "phoneNumber")]
        public String PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        [JsonProperty(PropertyName = "countrycode")]
        public String Countrycode
        {
            get { return _countrycode; }
            set { _countrycode = value; }
        }

        [JsonProperty(PropertyName = "roleName")]
        public String RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }

    }
}
