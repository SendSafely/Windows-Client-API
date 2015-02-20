using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Objects
{
    internal class ConnectionStrings
    {
        public static Dictionary<string, Endpoint> Endpoints = new Dictionary<string, Endpoint> {
            {"version",new Endpoint("/api/v1.1/config/version/{version}/", HTTPMethod.GET, "application/json")}, 
            {"verifyCredentials", new Endpoint("/api/v1.1/config/verify-credentials/", HTTPMethod.GET, "application/json")}, 
            {"createPackage", new Endpoint("/api/v1.1/package/", HTTPMethod.PUT, "application/json")},
            {"activePackages", new Endpoint("/api/v1.1/package/", HTTPMethod.GET, "application/json")},
            {"archivedPackages", new Endpoint("/api/v1.1/package/archived/", HTTPMethod.GET, "application/json")},
            {"addRecipient", new Endpoint("/api/v1.1/package/{packageId}/recipient/", HTTPMethod.PUT, "application/json")},
            {"addRecipientPhonenumber", new Endpoint("/api/v1.1/package/{packageId}/recipient/{recipientId}/", HTTPMethod.POST, "application/json")},
            {"createFileId", new Endpoint("/api/v1.1/package/{packageId}/file/", HTTPMethod.PUT, "application/json")},
            {"addMessage", new Endpoint("/api/v1.1/package/{packageId}/message/", HTTPMethod.PUT, "application/json")},
            {"uploadFile", new Endpoint("/api/v1.1/package/{packageId}/file/{fileId}/", HTTPMethod.POST, "multipart/form-data")},
            {"finalizePackage", new Endpoint("/api/v1.1/package/{packageId}/finalize/", HTTPMethod.POST, "application/json")},
            {"deleteTempPackage", new Endpoint("/api/v1.1/package/{packageId}/temp/", HTTPMethod.DELETE, "application/json")},
            {"deletePackage", new Endpoint("/api/v1.1/package/{packageId}/", HTTPMethod.DELETE, "application/json")},
            {"packageInformation", new Endpoint("/api/v1.1/package/{packageId}/", HTTPMethod.GET, "application/json")},
            {"updatePackage", new Endpoint("/api/v1.1/package/{packageId}/", HTTPMethod.POST, "application/json")},

            {"enterpriseInfo", new Endpoint("/api/v1.1/enterprise/", HTTPMethod.GET, "application/json")}
        };
    }
}
