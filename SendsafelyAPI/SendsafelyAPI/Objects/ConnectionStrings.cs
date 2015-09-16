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
            {"createPackage", new Endpoint("/api/v2.0/package/", HTTPMethod.PUT, "application/json")},
            {"activePackages", new Endpoint("/api/v2.0/package/", HTTPMethod.GET, "application/json")},
            {"archivedPackages", new Endpoint("/api/v2.0/package/archived/", HTTPMethod.GET, "application/json")},
            {"addRecipient", new Endpoint("/api/v2.0/package/{packageId}/recipient/", HTTPMethod.PUT, "application/json")},
            {"addRecipientPhonenumber", new Endpoint("/api/v2.0/package/{packageId}/recipient/{recipientId}/", HTTPMethod.POST, "application/json")},
            {"createFileId", new Endpoint("/api/v2.0/package/{packageId}/file/", HTTPMethod.PUT, "application/json")},
            {"addMessage", new Endpoint("/api/v2.0/package/{packageId}/message/", HTTPMethod.PUT, "application/json")},
            {"getMessage", new Endpoint("/api/v2.0/package/{packageId}/message/{checksum}/", HTTPMethod.GET, "application/json")},
            {"uploadFile", new Endpoint("/api/v2.0/package/{packageId}/file/{fileId}/", HTTPMethod.POST, "multipart/form-data")},
            {"downloadFile", new Endpoint("/api/v2.0/package/{packageId}/file/{fileId}/download/", HTTPMethod.POST, "application/json")},
            {"finalizePackage", new Endpoint("/api/v2.0/package/{packageId}/finalize/", HTTPMethod.POST, "application/json")},
            {"deleteTempPackage", new Endpoint("/api/v2.0/package/{packageId}/temp/", HTTPMethod.DELETE, "application/json")},
            {"deletePackage", new Endpoint("/api/v2.0/package/{packageId}/", HTTPMethod.DELETE, "application/json")},
            {"packageInformation", new Endpoint("/api/v2.0/package/{packageId}/", HTTPMethod.GET, "application/json")},
            {"updatePackage", new Endpoint("/api/v2.0/package/{packageId}/", HTTPMethod.POST, "application/json")},

            {"enterpriseInfo", new Endpoint("/api/v2.0/enterprise/", HTTPMethod.GET, "application/json")},

            {"startRegistration", new Endpoint("/auth-api/register/", HTTPMethod.PUT, "application/json")},
            {"finishRegistration", new Endpoint("/auth-api/register/{token}/", HTTPMethod.POST, "application/json")},
            {"finishPINRegistration", new Endpoint("/auth-api/pin-register/", HTTPMethod.POST, "application/json")},
            {"oauthRegistration", new Endpoint("/auth-api/generate-key/oauth/{token}/", HTTPMethod.PUT, "application/json")},
            {"generateKey", new Endpoint("/auth-api/generate-key/", HTTPMethod.PUT, "application/json")},
            {"generateKey2FA", new Endpoint("/auth-api/generate-key/{token}/", HTTPMethod.POST, "application/json")}

        };
    }
}
