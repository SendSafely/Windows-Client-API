using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    class PaginationResponse : StandardResponse
    {

        private Dictionary<String, String> _pagination;

        [JsonProperty(PropertyName = "pagination")]
        internal Dictionary<String, String> Pagination
        {
            get { return _pagination; }
            set { _pagination = value; }
        }
    }
}
