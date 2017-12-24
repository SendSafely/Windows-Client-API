using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely
{
    /// <summary>
    /// This object will contain information about a package. Once a package is created this object will be returned. 
    /// If it is passed along when adding recipients and files the object will be updated accordingly.
    /// </summary>
    public class User
    {
        private String _id;
        private String _email;
        private String _clientKey;
        private String _firstName;
        private String _lastName;
        private bool _publicKey;
        private int _packageLife;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public String ClientKey
        {
            get { return _clientKey; }
            set { _clientKey = value; }
        }

        public String FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public String LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public bool AllowPublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        public int PackageLife
        {
            get { return _packageLife; }
            set { _packageLife = value; }
        }

    }
}
