using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Objects
{
    enum HTTPMethod { PUT, POST, DELETE, GET };

    class Endpoint
    {
        private String path;
        private HTTPMethod method;
        private String contentType;

        public Endpoint(String path, HTTPMethod method, String contentType)
        {
            this.path = path;
            this.method = method;
            this.contentType = contentType;
        }

        public String Path
        {
            get { return path; }
            set { path = value; }
        }

        public String ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        internal HTTPMethod Method
        {
            get { return method; }
            set { method = value; }
        }

        internal Endpoint Clone()
        {
            Endpoint p = new Endpoint(path, method, contentType);
            return p;
        }
    }
}
