using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class AddRecipientsRequest
    {
        private List<String> _emails;

        [JsonProperty(PropertyName = "emails")]
        public List<String> Emails
        {
            get { return _emails; }
            set { _emails = value; }
        }
    }
}
