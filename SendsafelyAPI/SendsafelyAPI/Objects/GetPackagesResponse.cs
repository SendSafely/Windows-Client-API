using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GetPackagesResponse
    {
        private APIResponse _response;
        private List<PackageDTO> _packages;

        [JsonProperty(PropertyName = "response")]
        internal APIResponse Response
        {
            get { return _response; }
            set { _response = value; }
        }

        [JsonProperty(PropertyName = "packages")]
        public List<PackageDTO> Packages
        {
            get { return _packages; }
            set { _packages = value; }
        }
    }

    
}
