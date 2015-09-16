using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely
{
    public class EnterpriseInformation
    {

        private String host;
        private String systemName;
        private bool allowUndisclosedRecipients;
        private bool outlookBeta;

        public String Host
        {
            get { return host; }
            set { host = value; }
        }

        public String SystemName
        {
            get { return systemName; }
            set { systemName = value; }
        }

        public bool AllowUndisclosedRecipients
        {
            get { return allowUndisclosedRecipients; }
            set { allowUndisclosedRecipients = value; }
        }

        public bool OutlookBeta
        {
            get { return outlookBeta; }
            set { outlookBeta = value; }
        }

    }
}
