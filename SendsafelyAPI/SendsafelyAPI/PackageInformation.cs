using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely
{
    /// <summary>
    /// This object will contain information about a package. Once a package is created this object will be returned. 
    /// If it is passed along when adding recipients and files the object will be updated accordingly.
    /// </summary>
    public class PackageInformation
    {
        private String packageId;
        private String packageCode;
        private String keyCode;
        private String serverSecret;
        private bool needsApprover;
        private List<Recipient> recipients;
        private List<File> files;
        private List<String> approvers;
        private int life;

        /// <summary>
        /// The package ID for the given package.
        /// </summary>
        public String PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }

        /// <summary>
        /// The package code for the given package. The package code is a part of the URL that must be sent to the recipients.
        /// </summary>
        public String PackageCode
        {
            get { return packageCode; }
            set { packageCode = value; }
        }

        /// <summary>
        /// The keycode for the package. This key should always be kept client side and never be sent to the server. 
        /// The keycode makes up one part of the encryption key.
        /// </summary>
        public String KeyCode
        {
            get { return keyCode; }
            set { keyCode = value; }
        }

        /// <summary>
        /// The server secret makes together with the keycode up the encryption key. The server secret is specific 
        /// to a package and passed from the server.
        /// </summary>
        public String ServerSecret
        {
            get { return serverSecret; }
            set { serverSecret = value; }
        }

        /// <summary>
        /// NeedsApprover will be true when a package needs to add at least one approver before the package can be finalized.
        /// If the package is finalized without the approver, an exception will be thrown.
        /// </summary>
        public bool NeedsApprover
        {
            get { return needsApprover; }
            set { needsApprover = value; }
        }
        
        /// <summary>
        /// A list of recipients that are currently attached to the package.
        /// </summary>
        public List<Recipient> Recipients
        {
            get { return recipients; }
            set { recipients = value; }
        }

        /// <summary>
        /// A list of files that are currently attached to the package.
        /// </summary>
        public List<File> Files
        {
            get { return files; }
            set { files = value; }
        }

        /// <summary>
        /// A list of approvers that are currently attached to the package.
        /// </summary>
        public List<String> Approvers
        {
            get { return approvers; }
            set { approvers = value; }
        }

        /// <summary>
        /// The current package life. The package life determines for how long the package 
        /// should be available to the recipients. It's measured in days.
        /// </summary>
        public int Life
        {
            get { return life; }
            set { life = value; }
        }

    }
}
