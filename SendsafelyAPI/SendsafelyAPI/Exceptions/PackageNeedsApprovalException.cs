using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Exceptions
{
    /// <summary>
    /// Thrown when a package approval is needed in order for the recipients to be able to access the package. 
    /// When this exception is thrown the approvers must be notified so they can download and approve the other recipients.
    /// </summary>
    [Serializable]
    public class PackageNeedsApprovalException : BaseException
    {
        private String _link;
        private List<String> _approvers;

        public PackageNeedsApprovalException(List<String> approvers)
        {
            this._approvers = approvers;
        }

        public List<String> Approvers
        {
            get { return _approvers; }
            set { _approvers = value; }
        }

        public String Link
        {
            get { return _link; }
            set { _link = value; }
        }
    }
}
