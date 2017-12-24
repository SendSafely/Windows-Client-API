using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddRecipientsResponse : StandardResponse
    {
        private List<RecipientResponse> recipients;

        [JsonProperty(PropertyName = "recipients")]
        public List<RecipientResponse> Recipients
        {
            get { return recipients; }
            set { recipients = value; }
        }
    }
}
