using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class RecipientResponse : StandardResponse
    {
        private String recipientId;
        private String email;
        private Boolean needsApproval;
        private List<PhoneNumber> phonenumbers;
        private List<ConfirmationResponse> confirmations;
        private String autoEnabledNumber;


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

        [JsonProperty(PropertyName = "needsApproval")]
        public Boolean NeedsApproval
        {
            get { return needsApproval; }
            set { needsApproval = value; }
        }

        [JsonProperty(PropertyName = "phonenumbers")]
        public List<PhoneNumber> Phonenumbers
        {
            get { return phonenumbers; }
            set { phonenumbers = value; }
        }

        [JsonProperty(PropertyName = "confirmations")]
        public List<ConfirmationResponse> Confirmations
        {
            get { return confirmations; }
            set { confirmations = value; }
        }

        [JsonProperty(PropertyName = "autoEnabledNumber")]
        public String AutoEnabledNumber
        {
            get { return autoEnabledNumber; }
            set { autoEnabledNumber = value; }
        }
    }
}
