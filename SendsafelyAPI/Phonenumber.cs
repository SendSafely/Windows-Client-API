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
    public class PhoneNumber
    {
        private int countryCode;
        private String number;

        /// <summary>
        /// The phone numbers country code.
        /// </summary>
        [JsonProperty(PropertyName = "countryCode")]
        public int CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }

        /// <summary>
        /// The phone number itself.
        /// </summary>
        [JsonProperty(PropertyName = "phonenumber")]
        public String Number
        {
            get { return number; }
            set { number = value; }
        }

    }
}
