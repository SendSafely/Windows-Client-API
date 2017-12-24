using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely
{
    /// <summary>
    /// An object referencing a phone number. Contains two public variables, a CountryCode and a phonenumber.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PublicKeyRaw
    {
        private String _id;
        private String _key;

        /// <summary>
        /// The phone numbers country code.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public String ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// The phone number itself.
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

    }
}
