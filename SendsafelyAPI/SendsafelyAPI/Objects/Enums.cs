using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Objects
{
    /// <summary>
    /// A list of responses that will be returned by the server. Every server response should contain an APIResponse
    /// </summary>
    enum APIResponse
    {
        SUCCESS,
        FAIL,
        OUTDATED_VERSION,
        LIMIT_EXCEEDED,
        GUESTS_PROHIBITED,
        DENIED,
        INVALID_EMAIL,
        INVALID_FILE_NAME,
        DUPLICATE_EMAIL,
        APPROVER_REQUIRED,
        PACKAGE_NEEDS_APPROVAL,
        AUTHENTICATION_FAILED,
        UNKNOWN_PACKAGE,
        INVALID_INPUT,
        INVALID_RECIPIENT
    };

    /// <summary>
    /// An enum describing if there is an update available for the API.
    /// </summary>
    public enum Version { 
        /// <summary>
        /// The API is up to date. No updates are available.
        /// </summary>
        OK, 
        /// <summary>
        /// There is an upgrade available and it is recommended that a new version is installed. 
        /// The current version will still function but might be deprecated in the future.
        /// </summary>
        UPGRADE_AVAILABLE,
        /// <summary>
        /// A new version of the API must be downloaded for the API to be fully functioning.
        /// </summary>
        OUTDATED_VERSION };
}
