using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


namespace SendSafely
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PackageSearchResults
    {
        List<PackageInformation> packages;
    	bool capped;

        [JsonProperty(PropertyName = "packages")]
        public List<PackageInformation> Packages
        {
            get { return packages; }
            set { packages = value; }
        }

        [JsonProperty(PropertyName = "capped")]
        public bool Capped
        {
            get { return capped; }
            set { capped = value; }
        }
    }
}