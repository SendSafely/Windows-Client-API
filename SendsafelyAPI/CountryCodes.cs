using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely
{
    public class CountryCodes
    {
        /// <summary>
        /// All available country codes. SendSafely can only send text messages to phone numbers belonging to these countries.
        /// </summary>
        public enum CountryCode
        {
            US = 1, 
            GB = 44, 
            AU = 61,
            AT = 43,
            BE = 32,
            BR = 55,
            CN = 86,
            DK = 45,
            FI = 358,
            FR = 33,
            DE = 49,
            IN = 91,
            IQ = 964,
            IE = 353,
            IT = 39,
            JO = 962,
            KW = 965,
            LY = 218,
            NL = 31,
            NZ = 64,
            OM = 968,
            PL = 48,
            PT = 351,
            QA = 974,
            RU = 7,
            SA = 966,
            ES = 34,
            SE = 46,
            CH = 41,
            AE = 971,
            OT
        };

        /// <summary>
        /// Will, given a CountryCode return the country name.
        /// </summary>
        public static Dictionary<CountryCode, string> list = new Dictionary<CountryCode, string> { 
            { CountryCode.US, "United States" },
            { CountryCode.GB, "United Kingdom" }, 
            { CountryCode.AU, "Australia" }, 
            { CountryCode.AT, "Austria" }, 
            { CountryCode.BE, "Belgium" }, 
            { CountryCode.BR, "Brazil" }, 
            { CountryCode.CN, "China" }, 
            { CountryCode.DK, "Denmark" }, 
            { CountryCode.FI, "Finland" }, 
            { CountryCode.FR, "France" }, 
            { CountryCode.DE, "Germany" }, 
            { CountryCode.IN, "India" }, 
            { CountryCode.IQ, "Iraq" }, 
            { CountryCode.IE, "Ireland" },
            { CountryCode.IT, "Italy" },
            { CountryCode.JO, "Jordan" },
            { CountryCode.KW, "Kuwait" },
            { CountryCode.LY, "Libya" },
            { CountryCode.NL, "Netherlands" },
            { CountryCode.NZ, "New Zealan" },
            { CountryCode.OM, "Oman" },
            { CountryCode.PL, "Poland" },
            { CountryCode.PT, "Portugal" },
            { CountryCode.QA, "Qatar" },
            { CountryCode.RU, "Russia" },
            { CountryCode.SA, "Saudi Arabia" },
            { CountryCode.ES, "Spain" },
            { CountryCode.SE, "Sweden" },
            { CountryCode.CH, "Switzerland" },
            { CountryCode.AE, "United Arab Emirates" },
            { CountryCode.OT, "Other" }
        };
    }
}
