using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class UpdatePackageRequest
    {
        private int _life;

        [JsonProperty(PropertyName = "life")]
        public int Life
        {
            get { return _life ; }
            set { _life = value; }
        }
    }
}
