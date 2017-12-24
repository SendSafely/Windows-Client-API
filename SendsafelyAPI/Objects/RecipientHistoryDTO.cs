using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class RecipientHistoryDTO
    {
        private String _packageId;
        private String _packageUserName;
        private String _packageUserId;
        private int _packageState;
        private String _packageStateStr;
        private String _packageStateColor;
        private int _packageLife;
        private String _packageUpdateTimestampStr;
        private String _packageCode;
        private String _packageOS;
        private Recipient _packageRecipientResponse;
        private List<String> _files;
        private bool _packageContainsMessage;
        
        [JsonProperty(PropertyName = "packageId")]
        internal String PackageID
        {
            get { return _packageId; }
            set { _packageId = value; }
        }

        [JsonProperty(PropertyName = "packageUserName")]
        internal String PackageUserName
        {
            get { return _packageUserName; }
            set { _packageUserName = value; }
        }

        [JsonProperty(PropertyName = "packageUserId")]
        internal String PackageUserId
        {
            get { return _packageUserId; }
            set { _packageUserId = value; }
        }

        [JsonProperty(PropertyName = "packageState")]
        internal int PackageState
        {
            get { return _packageState; }
            set { _packageState = value; }
        }

        [JsonProperty(PropertyName = "packageStateStr")]
        internal String PackageStateStr
        {
            get { return _packageStateStr; }
            set { _packageStateStr = value; }
        }

        [JsonProperty(PropertyName = "packageStateColor")]
        internal String PackageStateColor
        {
            get { return _packageStateColor; }
            set { _packageStateColor = value; }
        }

        [JsonProperty(PropertyName = "packageLife")]
        internal int PackageLife
        {
            get { return _packageLife; }
            set { _packageLife = value; }
        }

        [JsonProperty(PropertyName = "packageUpdateTimestampStr")]
        internal String PackageUpdateTimestampStr
        {
            get { return _packageUpdateTimestampStr; }
            set { _packageUpdateTimestampStr = value; }
        }

        [JsonProperty(PropertyName = "packageCode")]
        internal String PackageCode
        {
            get { return _packageCode; }
            set { _packageCode = value; }
        }

        [JsonProperty(PropertyName = "packageOS")]
        internal String PackageOS
        {
            get { return _packageOS; }
            set { _packageOS = value; }
        }

        [JsonProperty(PropertyName = "packageRecipientResponse")]
        internal Recipient PackageRecipientResponse
        {
            get { return _packageRecipientResponse; }
            set { _packageRecipientResponse = value; }
        }

        [JsonProperty(PropertyName = "filenames")]
        internal List<String> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        [JsonProperty(PropertyName = "packageContainsMessage")]
        internal bool PackageContainsMessage
        {
            get { return _packageContainsMessage; }
            set { _packageContainsMessage = value; }
        }
    }
}
