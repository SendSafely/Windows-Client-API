using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Objects
{
    public class ActivityLogEntry
    {
        private String activityLogId;
        private DateTime timestamp;
        private String timestampStr;
        private String ipAddress;
        private String packageId;
        private String targetId;
        private String actionDescription;
        private String action;
        private UserDTO user;

        public String ActivityLogId
        {
            get { return activityLogId; }
            set { activityLogId = value; }
        }
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
        public String TimestampStr
        {
            get { return timestampStr; }
            set { timestampStr = value; }
        }
        public String IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        public String PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }
        public String TargetId
        {
            get { return targetId; }
            set { targetId = value; }
        }
        public String ActionDescription
        {
            get { return actionDescription; }
            set { actionDescription = value; }
        }
        public String Action
        {
            get { return action; }
            set { action = value; }
        }
        public UserDTO User
        {
            get { return user; }
            set { user = value; }
        }
    }
}
