using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Objects
{
    internal class ConnectionStrings
    {
        public static Dictionary<string, Endpoint> Endpoints = new Dictionary<string, Endpoint> {
            {"version",new Endpoint("/api/v2.0/config/version/{version}/", HTTPMethod.GET, "application/json")}, 
            {"verifyCredentials", new Endpoint("/api/v2.0/config/verify-credentials/", HTTPMethod.GET, "application/json")},
            {"userInformation", new Endpoint("/api/v2.0/user/", HTTPMethod.GET, "application/json")},
            {"createPackage", new Endpoint("/api/v2.0/package/", HTTPMethod.PUT, "application/json")},
            {"activePackages", new Endpoint("/api/v2.0/package/", HTTPMethod.GET, "application/json")},
            {"receivedPackages", new Endpoint("/api/v2.0/package/received/", HTTPMethod.GET, "application/json")},
            {"archivedPackages", new Endpoint("/api/v2.0/package/archived/", HTTPMethod.GET, "application/json")},
            {"organizationPackages", new Endpoint("/api/v2.0/package/organization/", HTTPMethod.POST, "application/json")},
            {"organizationPackagesSearch", new Endpoint("/api/v2.0/package/organization/search", HTTPMethod.POST, "application/json") },
            {"addRecipient", new Endpoint("/api/v2.0/package/{packageId}/recipient/", HTTPMethod.PUT, "application/json")},
            {"addRecipientPhonenumber", new Endpoint("/api/v2.0/package/{packageId}/recipient/{recipientId}/", HTTPMethod.POST, "application/json")},
            {"createFileId", new Endpoint("/api/v2.0/package/{packageId}/file/", HTTPMethod.PUT, "application/json")},
            {"addMessage", new Endpoint("/api/v2.0/package/{packageId}/message/", HTTPMethod.PUT, "application/json")},
            {"getMessage", new Endpoint("/api/v2.0/package/{packageId}/message/{checksum}/", HTTPMethod.GET, "application/json")},
            {"uploadFile", new Endpoint("/api/v2.0/package/{packageId}/file/{fileId}/", HTTPMethod.POST, "multipart/form-data")},
            {"uploadFileInDirectory", new Endpoint("/api/v2.0/package/{packageId}/{directoryId}/file/{fileId}/", HTTPMethod.POST, "multipart/form-data")},
            {"fileInformation", new Endpoint("/api/v2.0/package/{packageId}/directory/{directoryId}/file/{fileId}/", HTTPMethod.GET, "application/json")},
            
            {"deleteFile", new Endpoint("/api/v2.0/package/{packageId}/directory/{directoryId}/file/{fileId}/", HTTPMethod.DELETE, "application/json")},
            {"moveFile", new Endpoint("/api/v2.0/package/{packageId}/directory/{directoryId}/file/{fileId}/", HTTPMethod.POST, "application/json")},
            {"downloadFileFromDirectory", new Endpoint("/api/v2.0/package/{packageId}/directory/{directoryId}/file/{fileId}/download/", HTTPMethod.POST, "application/json")},
            {"downloadFile", new Endpoint("/api/v2.0/package/{packageId}/file/{fileId}/download/", HTTPMethod.POST, "application/json")},
            {"finalizePackage", new Endpoint("/api/v2.0/package/{packageId}/finalize/", HTTPMethod.POST, "application/json")},
            {"deleteTempPackage", new Endpoint("/api/v2.0/package/{packageId}/temp/", HTTPMethod.DELETE, "application/json")},
            {"deletePackage", new Endpoint("/api/v2.0/package/{packageId}/", HTTPMethod.DELETE, "application/json")},
            {"packageInformation", new Endpoint("/api/v2.0/package/{packageId}/", HTTPMethod.GET, "application/json")},
            {"updatePackage", new Endpoint("/api/v2.0/package/{packageId}/", HTTPMethod.POST, "application/json")},

            {"directoryInformation", new Endpoint("/api/v2.0/package/{packageId}/directory/{directoryId}", HTTPMethod.GET, "application/json")},
            {"createDirectory", new Endpoint("/api/v2.0/package/{packageId}/directory/{directoryId}/subdirectory/", HTTPMethod.PUT, "application/json")},
            {"deleteDirectory", new Endpoint("/api/v2.0/package/{packageId}/directory/{directoryId}/", HTTPMethod.DELETE, "application/json")},
            {"moveDirectory",   new Endpoint("/api/v2.0/package/{packageId}/move/{sourcedirectoryId}/{targetdirectoryId}/", HTTPMethod.POST, "application/json") },
            {"renameDirectory", new Endpoint("/api/v2.0/package/{packageId}/directory/{directoryId}/", HTTPMethod.POST, "application/json") },

            {"recipientInfo", new Endpoint("/api/v2.0/recipient/history/{userEmail}",HTTPMethod.GET, "application/json")},
            {"updateRecipient", new Endpoint("/api/v2.0/package/{packageId}/recipient/{recipientId}/", HTTPMethod.POST, "application/json")},
            {"getRecipient", new Endpoint("/api/v2.0/package/{packageId}/recipient/{recipientId}/", HTTPMethod.GET, "application/json")},
            {"getActivityLog", new Endpoint("/api/v2.0/package/{packageId}/activityLog/", HTTPMethod.POST, "application/json")},

            {"getPublicKeys", new Endpoint("/api/v2.0/package/{packageId}/public-keys/", HTTPMethod.GET, "application/json")},
            {"addEncryptedKeycode", new Endpoint("/api/v2.0/package/{packageId}/link/{publicKeyId}/", HTTPMethod.PUT, "application/json")},
            {"addPublicKey", new Endpoint("/api/v2.0/public-key/", HTTPMethod.PUT, "application/json")},
            {"revokePublicKey", new Endpoint("/api/v2.0/public-key/{publicKeyId}/", HTTPMethod.DELETE, "application/json")},
            {"getKeycode", new Endpoint("/api/v2.0/package/{packageId}/link/{publicKeyId}/", HTTPMethod.GET, "application/json")},

            {"enterpriseInfo", new Endpoint("/api/v2.0/enterprise/", HTTPMethod.GET, "application/json")},

            {"startRegistration", new Endpoint("/auth-api/register/", HTTPMethod.PUT, "application/json")},
            {"finishRegistration", new Endpoint("/auth-api/register/{token}/", HTTPMethod.POST, "application/json")},
            {"finishPINRegistration", new Endpoint("/auth-api/pin-register/", HTTPMethod.POST, "application/json")},
            {"oauthRegistration", new Endpoint("/auth-api/generate-key/oauth/{token}/", HTTPMethod.PUT, "application/json")},
            {"generateKey", new Endpoint("/auth-api/generate-key/", HTTPMethod.PUT, "application/json")},
            {"generateKey2FA", new Endpoint("/auth-api/generate-key/{token}/", HTTPMethod.POST, "application/json")},

            {"addDropzoneRecipient", new Endpoint("/api/v2.0/user/dropzone-recipients/", HTTPMethod.PUT, "application/json")},
            {"getDropzoneRecipients", new Endpoint("/api/v2.0/user/dropzone-recipients/", HTTPMethod.GET, "application/json")},
            {"removeDropzoneRecipient", new Endpoint("/api/v2.0/user/dropzone-recipients/", HTTPMethod.DELETE, "application/json")},
            {"createContactGroup", new Endpoint("/api/v2.0/group/", HTTPMethod.PUT, "application/json")},
            {"deleteContactGroup", new Endpoint("/api/v2.0/group/{groupId}/", HTTPMethod.DELETE, "application/json")},
            {"addContactGroupUser", new Endpoint("/api/v2.0/group/{groupId}/user/", HTTPMethod.PUT, "application/json")},
            {"removeUserFromContactGroup", new Endpoint("/api/v2.0/group/{groupId}/{userId}/", HTTPMethod.DELETE, "application/json")},
            {"getContactGroups", new Endpoint("/api/v2.0/user/groups/", HTTPMethod.GET, "application/json")},
            {"getEnterpriseContactGroups", new Endpoint("/api/v2.0/enterprise/groups/", HTTPMethod.GET, "application/json")},
            {"addRecipients", new Endpoint("/api/v2.0/package/{packageId}/recipients/", HTTPMethod.PUT, "application/json")},
            {"addContactGroupsToPackage", new Endpoint("/api/v2.0/package/{packageId}/group/{groupId}/", HTTPMethod.PUT, "application/json")},
            {"removeContactGroupsToPackage", new Endpoint("/api/v2.0/package/{packageId}/group/{groupId}/", HTTPMethod.DELETE, "application/json")},
            {"removeRecipient", new Endpoint("/api/v2.0/package/{packageId}/recipient/{recipientId}/", HTTPMethod.DELETE, "application/json")},
            {"notifyRecipients", new Endpoint("/api/v2.0/package/{packageId}/notify", HTTPMethod.POST, "application/json") },
        };
    }
}
