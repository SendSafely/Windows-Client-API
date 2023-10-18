using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    class GetOrganizationPakagesResponse : PaginationResponse
    {

        List<PackageDTO> packages;
        bool capped;

        [JsonProperty(PropertyName = "packages")]
        public List<PackageDTO> Packages
        {
            get { return packages; }
            set { packages = value; }
        }

        [Obsolete("capped is deprecated, please use Pagination property instead)",false)]
        [JsonProperty(PropertyName = "capped")]
        public bool Capped
        {
            get { return capped; }
            set { capped = value; }
        }
    }
}
