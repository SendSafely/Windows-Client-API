using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely
{
    /// <summary>
    /// A class describing a sendsafely recipient. This class makes up of an email, 
    /// a unique ID and a flag indicating if approval is needed for the recipient.
    /// </summary>
    public class Recipient
    {
        private String recipientId;
        private String email;
        private bool needsApproval;
        private List<String> approvers;
        private List<PhoneNumber> phonenumbers;
        private List<Confirmation> confirmations;
        private String roleName;

        /// <summary>
        /// The recipient ID. Each recipient is given a unique ID once it's added. 
        /// Use this ID to update the recipient in the future. The ID is unique to every package.
        /// </summary>
        public String RecipientId
        {
            get { return recipientId; }
            set { recipientId = value; }
        }

        /// <summary>
        /// The email belonging to the recipient.
        /// </summary>
        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// A flag indicating approval is needed or not for the recipient. The value of this flag will 
        /// depend on your enterprise settings as well as if the email belongs to a domain outside 
        /// of the organization or not.
        /// </summary>
        public bool NeedsApproval
        {
            get { return needsApproval; }
            set { needsApproval = value; }
        }

        /// <summary>
        /// A list of all possible approvers for the given recipient.
        /// </summary>
        public List<String> Approvers
        {
            get { return approvers; }
            set { approvers = value; }
        }

        /// <summary>
        /// A list of all phonenumbers that was used for this recipient in the past.
        /// </summary>
        public List<PhoneNumber> PhoneNumbers
        {
            get { return phonenumbers; }
            set { phonenumbers = value; }
        }

        /// <summary>
        /// A list of all confirmations for the recipient. A confirmation will be added as soon as 
        /// a recipient has downloaded one or more files from the item.
        /// </summary>
        public List<Confirmation> Confirmations
        {
            get { return confirmations; }
            set { confirmations = value; }
        }

        /// <summary>
        /// The role name associate with the recipient.
        /// </summary>
        public String RoleName 
        {
            get { return roleName; }
            set { roleName = value; }
        }
    }
}
