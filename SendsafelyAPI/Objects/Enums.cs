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
        TOKEN_EXPIRED,
        INVALID_RECIPIENT,
        PASSWORD_COMPLEXITY,
        TWO_FA_REQUIRED,
        TWO_FA_ENFORCED,
        AUTH_FORBIDDEN,
        INVALID_TOKEN,
        PIN_RESEND,
        USER_ALREADY_EXISTS,
        USER_NOT_FOUND,
        UNKNOWN_ENDPOINT
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

    public enum PackageStatus
    {
        ACTIVE, 
    	EXPIRED, 
    	ARCHIVED
    };

    public enum PackageState : int
    {
        PACKAGE_STATE_DELETED_PARTIALLY_COMPLETE = -4,
        PACKAGE_STATE_DELETED_INCOMPLETE=-3,
        PACKAGE_STATE_TEMP=-2,
        PACKAGE_STATE_DELETED_COMPLETE=-1,
        PACKAGE_STATE_IN_PROGRESS=0,
        PACKAGE_STATE_EXPIRED_INCOMPLETE=1,
        PACKAGE_STATE_EXPIRED_COMPLETE=2,
        PACKAGE_STATE_ACTIVE_COMPLETE=3,
        PACKAGE_STATE_ACTIVE_INCOMPLETE=4,
        PACKAGE_STATE_ACTIVE_PARTIALLY_COMPLETE=5,
        PACKAGE_STATE_EXPIRED_PARTIALLY_COMPLETE=6
    }
}
