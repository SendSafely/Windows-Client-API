using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GetPackagesResponse : PaginationResponse
    {

        private List<PackageDTO> _packages;

        [JsonProperty(PropertyName = "packages")]
        public List<PackageDTO> Packages
        {
            get { return _packages; }
            set { _packages = value; }
        }
    }

    
}
