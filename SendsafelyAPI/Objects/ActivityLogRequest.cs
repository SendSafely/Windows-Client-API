using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class ActivityLogRequest
    {
        [JsonProperty(PropertyName = "rowIndex")]
        public long RowIndex { get; set; }
    }
}
