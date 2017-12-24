using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely
{
    public class RecipientHistory
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

        public String PackageID
        {
            get { return _packageId; }
            set { _packageId = value; }
        }
        
        public String PackageUserName
        {
            get { return _packageUserName; }
            set { _packageUserName = value; }
        }
        
        public String PackageUserId
        {
            get { return _packageUserId; }
            set { _packageUserId = value; }
        }
        
        public int PackageState
        {
            get { return _packageState; }
            set { _packageState = value; }
        }
        
        public String PackageStateStr
        {
            get { return _packageStateStr; }
            set { _packageStateStr = value; }
        }
        
        public String PackageStateColor
        {
            get { return _packageStateColor; }
            set { _packageStateColor = value; }
        }
        
        public int PackageLife
        {
            get { return _packageLife; }
            set { _packageLife = value; }
        }
        
        public String PackageUpdateTimestampStr
        {
            get { return _packageUpdateTimestampStr; }
            set { _packageUpdateTimestampStr = value; }
        }
        
        public String PackageCode
        {
            get { return _packageCode; }
            set { _packageCode = value; }
        }
        
        public String PackageOS
        {
            get { return _packageOS; }
            set { _packageOS = value; }
        }
        
        public Recipient PackageRecipientResponse
        {
            get { return _packageRecipientResponse; }
            set { _packageRecipientResponse = value; }
        }
        
        public List<String> Files
        {
            get { return _files; }
            set { _files = value; }
        }
        
        public bool PackageContainsMessage
        {
            get { return _packageContainsMessage; }
            set { _packageContainsMessage = value; }
        }
    }
}
