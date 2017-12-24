using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Utilities.Encoders;

namespace SendSafely.Utilities
{
    class EncodingUtil
    {

        public static String Base64Encode(byte[] data)
        {
            String dataStr = Convert.ToBase64String(data);

            // Make it web safe
            dataStr = dataStr.Replace('+', '-');
            dataStr = dataStr.Replace('/', '_');
            // Remove '=' since it's not URL safe
            dataStr = dataStr.Replace("=", "");

            return dataStr;
        }

        public static String HexEncode(byte[] value)
        {
            return System.Text.Encoding.UTF8.GetString(Hex.Encode(value));
        }

    }
}
