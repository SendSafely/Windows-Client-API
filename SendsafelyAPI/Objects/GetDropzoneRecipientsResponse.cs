using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GetDropzoneRecipientsResponse : StandardResponse
    {
        private List<String> recipientEmailAddresses;

        [JsonProperty(PropertyName = "recipientEmailAddresses")]
        public List<String> RecipientEmailAddresses
        {
            get { return recipientEmailAddresses; }
            set { recipientEmailAddresses = value; }
        }


    }

    
}
