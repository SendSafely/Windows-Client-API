using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GetRecipientResponse : StandardResponse
    {
        private bool approvalRequired; 
        private bool checkForPublicKeys; 
        private String recipientId; 
        private String email; 
        private List<String> approvers; 
        private List<PhoneNumber> phonenumbers; 
        private String autoEnabledNumber; 
        private String phoneNumber; 
        private String fullName;
        private bool smsAuth;
        private bool isPackageOwner;
        private List<PublicKeyRaw> publicKeys;

        [JsonProperty(PropertyName = "publicKeys")]
        public List<PublicKeyRaw> PublicKeys
        {
            get { return publicKeys; }
            set { publicKeys = value; }
        }


        [JsonProperty(PropertyName = "isPackageOwner")]
        public bool IsPackageOwner
        {
            get { return isPackageOwner; }
            set { isPackageOwner = value; }
        }
        [JsonProperty(PropertyName = "smsAuth")]
        public bool SmsAuth
        {
            get { return smsAuth; }
            set { smsAuth = value; }
        }

        [JsonProperty(PropertyName = "fullName")]
        public String FullName
        {
            get { return FullName; }
            set { FullName = value; }
        }


        [JsonProperty(PropertyName = "phoneNumber")]
        public String PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        [JsonProperty(PropertyName = "approvers")]
        public List<String> Approvers
        {
            get { return approvers; }
            set { approvers = value; }
        }

        [JsonProperty(PropertyName = "checkForPublicKeys")]
        public bool CheckForPublicKeys
        {
            get { return checkForPublicKeys; }
            set { checkForPublicKeys = value; }
        }

        [JsonProperty(PropertyName = "recipientId")]
        public String RecipientId
        {
            get { return recipientId; }
            set { recipientId = value; }
        }

        [JsonProperty(PropertyName = "email")]
        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        [JsonProperty(PropertyName = "approvalRequired")]
        public Boolean ApprovalRequired
        {
            get { return approvalRequired; }
            set { approvalRequired = value; }
        }

        [JsonProperty(PropertyName = "phonenumbers")]
        public List<PhoneNumber> Phonenumbers
        {
            get { return phonenumbers; }
            set { phonenumbers = value; }
        }

        [JsonProperty(PropertyName = "autoEnabledNumber")]
        public String AutoEnabledNumber
        {
            get { return autoEnabledNumber; }
            set { autoEnabledNumber = value; }
        }
    }
}
