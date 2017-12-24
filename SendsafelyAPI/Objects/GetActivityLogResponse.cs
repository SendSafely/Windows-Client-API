using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GetActivityLogResponse : StandardResponse
    {
        private List<ActivityLogEntry> activityLogEntries;
        private long dataCount;

        [JsonProperty(PropertyName = "activityLogEntries")]
        internal List<ActivityLogEntry> ActivityLogEntries
        {
            get { return activityLogEntries; }
            set { activityLogEntries = value; }
        }

        [JsonProperty(PropertyName = "dataCount")]
        internal long DataCount
        {
            get { return dataCount; }
            set { dataCount = value; }
        }

    }
}
