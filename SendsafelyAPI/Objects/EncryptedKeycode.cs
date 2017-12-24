using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely
{
    public class EncryptedKeycode
    {
        private String _id;
        private String _keycode;

        public String ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public String Keycode
        {
            get { return _keycode; }
            set { _keycode = value; }
        }

    }
}
