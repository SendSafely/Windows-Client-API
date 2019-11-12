using System;
using System.Collections.Generic;
using System.Text;
using SendSafely.Objects;
using SendSafely.Exceptions;
using SendSafely.Utilities;
using System.IO;

namespace SendSafely
{
    internal class PackageUtility
    {
        private Connection connection;
        private long SEGMENT_SIZE = 2621440;

        #region Constructors
        
        public PackageUtility(Connection connection)
        {
            this.connection = connection;
        }
        
        #endregion

        #region Public Functions
        public PackageInformation CreatePackage()
        {
            return CreatePackage(false, String.Empty);
        }

        public PackageInformation CreatePackage(Boolean isWorkspace)
        {
            return CreatePackage(true, String.Empty);
        }


        public PackageInformation CreatePackage(String email)
        {
            return CreatePackage(false, email);
        }

        public PackageInformation CreatePackage(Boolean isWorkspace, String email)
        {
            Endpoint p = ConnectionStrings.Endpoints["createPackage"].Clone();
            CreatePackageRequest request = new CreatePackageRequest();
            request.Vdr = isWorkspace;
            request.PackageUserEmail = email;
            CreatePackageResponse response = connection.Send<CreatePackageResponse>(p,request);

            Logger.Log("Response: " + response.Response);
            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            // Derive keycode
            CryptUtility cu = new CryptUtility();

            PackageInformation packageInfo = new PackageInformation();
            Logger.Log("Adding keycode to package: " + packageInfo.PackageId);
            packageInfo.KeyCode = cu.GenerateToken();
            connection.AddKeycode(response.PackageId, packageInfo.KeyCode);
            packageInfo = this.GetPackageInformation(response.PackageId);

            return packageInfo;
        }

        public String CreateDirectory(String packageId, String parentDirectoryId, String directoryName)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }
            if(parentDirectoryId == null)
            {
                throw new InvalidPackageException("Parent Directory ID can not be null");
            }

           Endpoint p = ConnectionStrings.Endpoints["createDirectory"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{directoryId}", parentDirectoryId);

            CreateDirectoryRequest request = new CreateDirectoryRequest();
            request.DirectoryName = directoryName;
            StandardResponse response = connection.Send<StandardResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return response.Message; //returns directoryId
        }

        public void DeleteDirectory(String packageId, String directoryId)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }
            if (directoryId == null)
            {
                throw new InvalidPackageException("Directory ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["deleteDirectory"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{directoryId}", directoryId);
            
            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public List<PackageInformation> GetActivePackages()
        {
            Endpoint p = ConnectionStrings.Endpoints["activePackages"].Clone();
            GetPackagesResponse response = connection.Send<GetPackagesResponse>(p);

            return Convert(response.Packages);
        }

        public List<RecipientHistory> GetRecipientHistory(String recipientEmail)
        {
            Endpoint p = ConnectionStrings.Endpoints["recipientInfo"].Clone();
            p.Path = p.Path.Replace("{userEmail}", recipientEmail);
            RecipientHistoryResponse response = connection.Send<RecipientHistoryResponse>(p);

            if (response.Response == APIResponse.INVALID_EMAIL)
            {
                throw new InvalidEmailException(response.Message);
            }
            else if (response.Response == APIResponse.DENIED)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
            return Convert(response.packages);
        }

        public List<PackageInformation> GetReceivedPackages()
        {
            Endpoint p = ConnectionStrings.Endpoints["receivedPackages"].Clone();
            GetPackagesResponse response = connection.Send<GetPackagesResponse>(p);
            return Convert(response.Packages);
        }


        public List<PackageInformation> GetArchivedPackages()
        {
            Endpoint p = ConnectionStrings.Endpoints["archivedPackages"].Clone();
            GetPackagesResponse response = connection.Send<GetPackagesResponse>(p);

            return Convert(response.Packages);
        }

        public PackageInformation GetPackageInformation(String packageId)
        {
            Endpoint p = ConnectionStrings.Endpoints["packageInformation"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            PackageInformationResponse response = connection.Send<PackageInformationResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new InvalidPackageException(response.Message);
            }

            return Convert(response);
        }

        public PackageInformation GetPackageInformationFromLink(String link)
        {
            return GetPackageInformationFromLink(new Uri(link));
        }

        public PackageInformation GetPackageInformationFromLink(Uri link)
        {
            try
            {
                String packageCode = getPackageCode(link);
                String keyCode = getKeyCode(link);

                PackageInformation pkgInfo = GetPackageInformation(packageCode);
                pkgInfo.KeyCode = keyCode;
                return pkgInfo;
            }
            catch (InvalidLinkException e)
            {
                throw new InvalidPackageException(e.Message);
            }
        }

        public bool UpdatePackageLife(String packageId, int life)
        {
            Endpoint p = ConnectionStrings.Endpoints["updatePackage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);

            UpdatePackageRequest req = new UpdatePackageRequest();
            req.Life = life;

            StandardResponse response = connection.Send<StandardResponse>(p, req);

            bool retVal = false;
            if (response.Response == APIResponse.DENIED)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
            if (response.Response == APIResponse.SUCCESS)
            {
                retVal = true;
            }

            return retVal;
        }

        public void UpdatePackageDescriptor(String packageId, String packageDescriptor)
        {
            Endpoint p = ConnectionStrings.Endpoints["updatePackage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);

            UpdatePackageDescriptorRequest req = new UpdatePackageDescriptorRequest();
            req.Label = packageDescriptor;

            StandardResponse response = connection.Send<StandardResponse>(p, req);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public void UpdateRecipientRole(String packageId, String recipientId, String role)
        {
            Endpoint p = ConnectionStrings.Endpoints["updateRecipient"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{recipientId}", recipientId);

            UpdateRecipientRequest req = new UpdateRecipientRequest();
            req.RoleName = role;

            StandardResponse response = connection.Send<StandardResponse>(p, req);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public Recipient GetRecipient(string packageId, string recipientId)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["getRecipient"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{recipientId}", recipientId);

            GetRecipientResponse response = connection.Send<GetRecipientResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
            return convert(response);
        }

        public Recipient AddRecipient(String packageId, String email)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["addRecipient"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);

            AddRecipientRequest request = new AddRecipientRequest();
            request.Email = email;
            request.AutoEnableSMS = false;
            AddRecipientResponse response = connection.Send<AddRecipientResponse>(p, request);

            if (response.Response == APIResponse.INVALID_EMAIL || response.Response == APIResponse.DUPLICATE_EMAIL)
            {
                throw new InvalidEmailException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            Recipient recipient = new Recipient();

            recipient.NeedsApproval = response.ApprovalRequired;
            recipient.Approvers = response.Approvers;
            recipient.RecipientId = response.RecipientId;
            recipient.PhoneNumbers = response.Phonenumbers;
            recipient.RoleName = response.RoleName;
            recipient.Email = email;

            if (recipient.PhoneNumbers == null)
            {
                recipient.PhoneNumbers = new List<PhoneNumber>();
            }

            if (recipient.Approvers == null)
            {
                recipient.Approvers = new List<String>();
            }

            return recipient;
        }

        public void AddDropzoneRecipient(String email)
        {

            Endpoint p = ConnectionStrings.Endpoints["addDropzoneRecipient"].Clone();

            AddDropzoneRecipientRequest request = new AddDropzoneRecipientRequest();
            request.UserEmail = email;
            StandardResponse response = connection.Send<StandardResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public List<String> GetDropzoneRecipients()
        {
            Endpoint p = ConnectionStrings.Endpoints["getDropzoneRecipients"].Clone();
            GetDropzoneRecipientsResponse response = connection.Send<GetDropzoneRecipientsResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
            return response.RecipientEmailAddresses;
        }

        public String GetMessage(String secureLink)
        {
            if (secureLink == null)
            {
                throw new InvalidLinkException("The secure link can not be null");
            }

            // Get the package and keycode from the secure link.
            String packageCode = getPackageCode(secureLink);
            String keyCode = getKeyCode(secureLink);

            VerifyKeycode(keyCode);

            CryptUtility cu = new CryptUtility();
            String checksum = cu.pbkdf2(keyCode, packageCode, 1024);

            // Get the package information from the server so we can get the package ID and server secret.
            PackageInformation packageInfo = this.GetPackageInformation(packageCode);

            // Get the encrypted message.
            Endpoint p = ConnectionStrings.Endpoints["getMessage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);
            p.Path = p.Path.Replace("{checksum}", checksum);
            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new InvalidLinkException("Failed to fetch the message from the server. Make sure the URL is incorrect.");
            }

            if (response.Message == null || response.Message.Length == 0)
            {
                // We have no message, return null
                return null;
            }

            // Finally, decrypt the message
            String key = CreateEncryptionKey(packageInfo.ServerSecret, keyCode);
            char[] passPhrase = key.ToCharArray();

            CryptUtility _cu = new CryptUtility();
            String message = _cu.DecryptMessage(response.Message, passPhrase);

            return message;
        }

        public File EncryptAndUploadFile(String packageId, String keycode, String path, ISendSafelyProgress progress, String uploadType)
        {
            return EncryptAndUploadFile(packageId, null, keycode, path, progress, uploadType);
        }

        public File EncryptAndUploadFile(String packageId, String directoryId, String keycode, String path, ISendSafelyProgress progress, String uploadType)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            connection.AddKeycode(packageId, keycode);

            // Get the updated package information.
            PackageInformation packageInfo = GetPackageInformation(packageId);
            packageInfo.KeyCode = keycode;
            return EncryptAndUploadFile(packageInfo, directoryId, path, progress, uploadType);
        }

        public File EncryptAndUploadFile(PackageInformation packageInfo, String directoryId, String path, ISendSafelyProgress progress, String uploadType)
        {
            if (packageInfo == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            VerifyKeycode(packageInfo.KeyCode);

            if (path == null || path.Equals(""))
            {
                throw new FileNotFoundException("Path can not be null or empty");
            }

            String key = CreateEncryptionKey(packageInfo.ServerSecret, packageInfo.KeyCode);
            char[] passPhrase = key.ToCharArray();
            System.IO.FileInfo unenCryptedFile = new System.IO.FileInfo(path);
            String filename = unenCryptedFile.Name;

            int parts = calculateParts(unenCryptedFile.Length);

            String fileId;
            Endpoint p;

            if (directoryId != null)
            {
                fileId = createFileId(packageInfo, directoryId, filename, unenCryptedFile.Length, parts, uploadType);
                p = ConnectionStrings.Endpoints["uploadFileInDirectory"].Clone();
                p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);
                p.Path = p.Path.Replace("{fileId}", fileId);
                p.Path = p.Path.Replace("{directoryId}", directoryId);
            } else
            {
                fileId = createFileId(packageInfo, filename, unenCryptedFile.Length, parts, uploadType);
                p = ConnectionStrings.Endpoints["uploadFile"].Clone();
                p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);
                p.Path = p.Path.Replace("{fileId}", fileId);
            }

            long approximateFilesize = unenCryptedFile.Length + (parts * 70);
            long uploadedSoFar = 0;

            long partSize = unenCryptedFile.Length / parts;

            long totalFilesize = unenCryptedFile.Length;
            long longDiff = 0;
            for (int i = 0; i < parts; i++)
            {
                longDiff += partSize;
            }
            long offset = totalFilesize - longDiff;

            using (FileStream readStream = unenCryptedFile.OpenRead())
            {
                for (int i = 1; i <= parts; i++ )
                {
                    System.IO.FileInfo segment = createSegment(readStream, (partSize + offset));
                    encryptAndUploadSegment(p, packageInfo, i, segment, fileId, passPhrase, uploadedSoFar, approximateFilesize, progress, uploadType);
                    uploadedSoFar += segment.Length;
                    segment.Delete();

                    // Offset is only for the first package.
                    offset = 0;
                }
            }
            
            File file = new File();
            file.FileId = fileId;
            file.FileName = filename;

            return file;
        }

        public void EncryptAndUploadMessage(String packageId, String keycode, String message, String applicationName)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            connection.AddKeycode(packageId, keycode);

            // Get the updated package information.
            PackageInformation packageInfo = GetPackageInformation(packageId);
            packageInfo.KeyCode = keycode;
            EncryptAndUploadMessage(packageInfo, message, applicationName);
        }

        public void EncryptAndUploadMessage(PackageInformation packageInfo, String message, String applicationName)
        {
            if (packageInfo == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            VerifyKeycode(packageInfo.KeyCode);

            message = (message == null) ? "" : message.Trim();

            String encryptedMessage = "";
            if (!message.Equals(""))
            {
                String key = CreateEncryptionKey(packageInfo.ServerSecret, packageInfo.KeyCode);
                char[] passPhrase = key.ToCharArray();

                CryptUtility _cu = new CryptUtility();
                encryptedMessage = _cu.EncryptMessage(message, passPhrase);
            }

            Endpoint p = ConnectionStrings.Endpoints["addMessage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);

            AddMessageRequest request = new AddMessageRequest();
            request.Message = encryptedMessage;
            request.UploadType = applicationName;

            StandardResponse response = connection.Send<StandardResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public FileInfo DownloadFile(String packageId, String directoryId, String fileId, String keycode, ISendSafelyProgress progress, String downloadAPI, String password)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            connection.AddKeycode(packageId, keycode);

            // Get the updated package information.
            PackageInformation packageInfo = GetPackageInformation(packageId);
            packageInfo.KeyCode = keycode;

            DownloadFileUtility downloadUtility;
            if (directoryId != null) //Workspace package
            {
                downloadUtility = new DownloadFileUtility(connection, GetDirectory(packageId, directoryId), packageInfo, progress, downloadAPI);
            }
            else //Classic package
            {
                downloadUtility = new DownloadFileUtility(connection, packageInfo, progress, downloadAPI, password);
            }

            return downloadUtility.downloadFile(fileId);
        }

        public String FinalizePackage(String packageId, String keycode)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("PackageId can not be null");
            }

            // Add the keycode in case we don't have it
            connection.AddKeycode(packageId, keycode);

            // Get the updated package information.
            PackageInformation packageInfo = GetPackageInformation(packageId);
            return FinalizePackage(packageInfo);
        }

        public String FinalizePackage(PackageInformation packageInfo)
        {
            FinalizePackageRequest request = new FinalizePackageRequest();
            return FinalizePackage(packageInfo, request);
        }

        public String FinalizePackage(PackageInformation packageInfo, FinalizePackageRequest request)
        {
            if (packageInfo == null)
            {
                throw new InvalidPackageException("PackageInformation can not be null");
            }

            VerifyKeycode(packageInfo.KeyCode);

            EncryptKeycodes(packageInfo);

            Endpoint p = ConnectionStrings.Endpoints["finalizePackage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);

            CryptUtility cu = new CryptUtility();
            request.Checksum = cu.pbkdf2(packageInfo.KeyCode, packageInfo.PackageCode, 1024);
            connection.AddKeycode(packageInfo.PackageId, packageInfo.KeyCode);

            FinalizePackageResponse response = connection.Send<FinalizePackageResponse>(p, request);

            if (response.Response == APIResponse.DENIED)
            {
                throw new PackageFinalizationException(response.Errors);    
            }
            if (response.Response == APIResponse.APPROVER_REQUIRED)
            {
                throw new ApproverRequiredException("Package needs an approver");
            }
            if (response.Response == APIResponse.PACKAGE_NEEDS_APPROVAL)
            {
                // We are approved at this point. We still return an exception so the user knows that the package requires an approver.
                PackageNeedsApprovalException pnae = new PackageNeedsApprovalException(response.Approvers);
                pnae.Link = response.Message + "#keyCode=" + packageInfo.KeyCode;
                throw pnae;
            }
            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return response.Message + "#keyCode=" + packageInfo.KeyCode;
        }

        public string FinalizeUndisclosedPackage(string packageId, string keycode)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("PackageId can not be null");
            }

            // Add the keycode in case we don't have it
            connection.AddKeycode(packageId, keycode);

            // Get the updated package information.
            PackageInformation packageInfo = GetPackageInformation(packageId);

            //set request object for the undisclosed package.
            FinalizePackageRequest request = new FinalizePackageRequest();
            request.UndisclosedRecipients = true;
            return FinalizePackage(packageInfo, request);
        }

        public string FinalizeUndisclosedPackage(string packageId, string password, string keycode)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("PackageId can not be null");
            }

            // Add the keycode in case we don't have it
            connection.AddKeycode(packageId, keycode);

            // Get the updated package information.
            PackageInformation packageInfo = GetPackageInformation(packageId);

            //set request object for the undisclosed package.
            FinalizePackageRequest request = new FinalizePackageRequest();
            request.UndisclosedRecipients = true;
            request.Password = password;
            return FinalizePackage(packageInfo, request);
        }

        public String FinalizeUndisclosedPackage(PackageInformation packageInfo, FinalizePackageRequest request)
        {
            if (packageInfo == null)
            {
                throw new InvalidPackageException("PackageInformation can not be null");
            }

            VerifyKeycode(packageInfo.KeyCode);

            EncryptKeycodes(packageInfo);

            Endpoint p = ConnectionStrings.Endpoints["finalizePackage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);

            CryptUtility cu = new CryptUtility();
            request.Checksum = cu.pbkdf2(packageInfo.KeyCode, packageInfo.PackageCode, 1024);
            connection.AddKeycode(packageInfo.PackageId, packageInfo.KeyCode);

            FinalizePackageResponse response = connection.Send<FinalizePackageResponse>(p, request);

            if (response.Response == APIResponse.DENIED)
            {
                throw new PackageFinalizationException(response.Errors);
            }
            if (response.Response == APIResponse.APPROVER_REQUIRED)
            {
                throw new ApproverRequiredException("Package needs an approver");
            }
            if (response.Response == APIResponse.PACKAGE_NEEDS_APPROVAL)
            {
                // We are approved at this point. We still return an exception so the user knows that the package requires an approver.
                PackageNeedsApprovalException pnae = new PackageNeedsApprovalException(response.Approvers);
                pnae.Link = response.Message + "#keyCode=" + packageInfo.KeyCode;
                throw pnae;
            }
            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return response.Message + "#keyCode=" + packageInfo.KeyCode;
        }


        public void DeleteTempPackage(String packageId)
        {
            Endpoint p = ConnectionStrings.Endpoints["deleteTempPackage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);

            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public void DeletePackage(String packageId)
        {
            Endpoint p = ConnectionStrings.Endpoints["deletePackage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);

            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public void AddRecipientPhonenumber(String packageId, String recipientId, String phonenumber, CountryCodes.CountryCode countrycode)
        {
            Endpoint p = ConnectionStrings.Endpoints["addRecipientPhonenumber"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{recipientId}", recipientId);

            UpdateRecipientRequest urr = new UpdateRecipientRequest();
            urr.Countrycode = countrycode.ToString();
            urr.PhoneNumber = phonenumber;

            StandardResponse response = connection.Send<StandardResponse>(p, urr);

            if (response.Response == APIResponse.INVALID_INPUT)
            {
                throw new InvalidPhonenumberException(response.Message);
            }
            else if (response.Response == APIResponse.INVALID_RECIPIENT)
            {
                throw new InvalidRecipientException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public void DeleteFile(String packageId, String directoryId, String fileId)
        {
            Endpoint p = ConnectionStrings.Endpoints["deleteFile"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{directoryId}", directoryId);
            p.Path = p.Path.Replace("{fileId}", fileId);
            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {   
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

        }

        public void MoveFile(String packageId, String fileId, String destinationDirectoryId)
        {
            Endpoint p = ConnectionStrings.Endpoints["moveFile"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{directoryId}", destinationDirectoryId);
            p.Path = p.Path.Replace("{fileId}", fileId);
            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

        }

        public Directory GetDirectory(String packageId, String directoryId)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }
            if (directoryId == null)
            {
                throw new InvalidPackageException("Directory ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["directoryInformation"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{directoryId}", directoryId);
            GetDirectoryResponse response = connection.Send<GetDirectoryResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return Convert(response);
        }

        public void MoveDirectory(String packageId, String sourceDirectoryId, String destinationDirectoryId)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }
            if (sourceDirectoryId == null)
            {
                throw new InvalidPackageException("Source Directory ID can not be null");
            }
            if (destinationDirectoryId == null)
            {
                throw new InvalidPackageException("Destination Directory ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["moveDirectory"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{sourcedirectoryId}", sourceDirectoryId);
            p.Path = p.Path.Replace("{targetdirectoryId}", destinationDirectoryId);

            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public void RenameDirectory(String packageId, String directoryId, String directoryName)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }
            if (directoryId == null)
            {
                throw new InvalidPackageException("Directory ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["renameDirectory"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{directoryId}", directoryId);

            RenameDirectoryRequest request = new RenameDirectoryRequest();
            request.DirectoryName = directoryName;

            StandardResponse response = connection.Send<StandardResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public void AddContactGroupToPackage(string packageId, string groupId)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["addContactGroupsToPackage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{groupId}", groupId);

            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public void RemoveContactGroupFromPackage(string packageId, string groupId)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["removeContactGroupsToPackage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{groupId}", groupId);

            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public void RemoveRecipient(string packageId, string recipientId)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["removeRecipient"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{recipientId}", recipientId);

            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public FileInformation GetFileInformation(String packageId, String directoryId, String fileId)
        {
            Endpoint p = ConnectionStrings.Endpoints["fileInformation"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            p.Path = p.Path.Replace("{directoryId}", directoryId);
            p.Path = p.Path.Replace("{fileId}", fileId);
            FileInformationResponse response = connection.Send<FileInformationResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return Convert(response);
        }

        public List<ActivityLogEntry> GetActivityLog(String packageId, int rowIndex)
        {
            Endpoint p = ConnectionStrings.Endpoints["getActivityLog"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);
            ActivityLogRequest request = new ActivityLogRequest();
            request.RowIndex = rowIndex;
            GetActivityLogResponse response = connection.Send<GetActivityLogResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return response.ActivityLogEntries;
        }

        public void RemoveDropzoneRecipient(string email)
        {

            Endpoint p = ConnectionStrings.Endpoints["removeDropzoneRecipient"].Clone();

            AddRemoveGroupUserRequest request = new AddRemoveGroupUserRequest();
            request.UserEmail = email;
            StandardResponse response = connection.Send<StandardResponse>(p, request);

            if (response.Response == APIResponse.GUESTS_PROHIBITED || response.Response == APIResponse.FAIL)
            {
                throw new InvalidEmailException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public String CreateContactGroup(String groupName)
        {

            Endpoint p = ConnectionStrings.Endpoints["createContactGroup"].Clone();

            AddContactGroupRequest request = new AddContactGroupRequest();
            request.GroupName = groupName;
            AddContactGroupResponse response = connection.Send<AddContactGroupResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
            return response.ContactGroupId;
        }

        public String CreateContactGroup(String groupName, String isEnterpriseGroup)
        {

            Endpoint p = ConnectionStrings.Endpoints["createContactGroup"].Clone();

            AddContactGroupRequest request = new AddContactGroupRequest();
            request.GroupName = groupName;
            request.IsEnterpriseGroup = isEnterpriseGroup;
            AddContactGroupResponse response = connection.Send<AddContactGroupResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
            return response.ContactGroupId;
        }

        public void DeleteContactGroup(string groupId)
        {

            Endpoint p = ConnectionStrings.Endpoints["deleteContactGroup"].Clone();
            p.Path = p.Path.Replace("{groupId}", groupId);
            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public String AddGroupUser(string groupId, string userEmail)
        {

            Endpoint p = ConnectionStrings.Endpoints["addContactGroupUser"].Clone();
            p.Path = p.Path.Replace("{groupId}", groupId);

            AddRemoveGroupUserRequest request = new AddRemoveGroupUserRequest();
            request.UserEmail = userEmail;
            AddGroupResponse response = connection.Send<AddGroupResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
            return response.UserId;
        }

        public void RemoveUserFromContactGroup(string groupId, string userId)
        {

            Endpoint p = ConnectionStrings.Endpoints["removeUserFromContactGroup"].Clone();
            p.Path = p.Path.Replace("{groupId}", groupId);
            p.Path = p.Path.Replace("{userId}", userId);
            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public List<ContactGroup> GetContactGroups(bool isEnterprise)
        {
            Endpoint p;
            if (isEnterprise)
            {
                p = ConnectionStrings.Endpoints["getEnterpriseContactGroups"].Clone();
            }
            else
            {
                p = ConnectionStrings.Endpoints["getContactGroups"].Clone();
            }

            GetUserGroupsResponse response = connection.Send<GetUserGroupsResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return response.ContactGroups;
        }

        public List<Recipient> AddRecipients(string packageId, List<String> emails)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            Endpoint p = ConnectionStrings.Endpoints["addRecipients"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);

            AddRecipientsRequest request = new AddRecipientsRequest();
            request.Emails = emails;
            AddRecipientsResponse response = connection.Send<AddRecipientsResponse>(p, request);

            if (response.Response == APIResponse.LIMIT_EXCEEDED)
            {
                throw new LimitExceededException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            List<Recipient> recipients = convert(response.Recipients);
            return recipients;
        }
        #endregion

        #region Private Functions

        private void EncryptKeycodes(PackageInformation pkgInfo)
        {
            List<PublicKeyRaw> publicKeys = GetPublicKeys(pkgInfo.PackageId);

            List<EncryptedKeycode> encryptedKeycodes = EncryptKeycodes(publicKeys, pkgInfo.KeyCode);
            UploadKeycodes(pkgInfo.PackageId, encryptedKeycodes);
        }

        private List<EncryptedKeycode> EncryptKeycodes(List<PublicKeyRaw> publicKeys, String keycode)
        {
            List<EncryptedKeycode> keycodes = new List<EncryptedKeycode>();
            foreach (PublicKeyRaw publicKey in publicKeys)
            {
                keycodes.Add(Create(publicKey, keycode));
            }
            return keycodes;
        }

        internal PackageSearchResults GetOrganizationPackages(DateTime? fromDate, DateTime? toDate, string sender, PackageStatus? status, string recipient, string fileName)
        {
            GetOrganizationPackagesRequest request = new GetOrganizationPackagesRequest();

            if (fromDate != null)
            {
                request.FromDate = fromDate.Value.ToString("MM/dd/yyyy");
            }
            if (toDate != null)
            {
                request.ToDate = toDate.Value.ToString("MM/dd/yyyy");
            }
            if (status != null)
            {
                request.Status = status.ToString();
            }
            request.Sender = sender;
            request.Recipient = recipient;
            request.Filename = fileName;

            Endpoint p = ConnectionStrings.Endpoints["organizationPackages"].Clone();
            GetOrganizationPakagesResponse response = connection.Send<GetOrganizationPakagesResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
             
            return Convert(response);
        }

        private PackageSearchResults Convert(GetOrganizationPakagesResponse response)
        {
            List<PackageInformation> packages = new List<PackageInformation>(response.Packages.Count);

            foreach (PackageDTO resp  in response.Packages)
            {
                packages.Add(convert(resp));
            }
            PackageSearchResults organizationPackages = new PackageSearchResults();
            organizationPackages.Packages = packages;
            organizationPackages.Capped = response.Capped;
            return organizationPackages;
        }

        private PackageInformation convert(PackageDTO obj)
        {
            PackageInformation info = new PackageInformation();
            info.Approvers = obj.Approvers;
            info.Life = obj.Life;
            info.NeedsApprover = obj.NeedsApprover;
            info.PackageCode = obj.PackageCode;
            info.PackageId = obj.PackageID;
            info.Recipients = ConvertRecipients(obj.Recipients);
            info.ServerSecret = obj.ServerSecret;
            info.PackageOwner = obj.PackageUserName;
            info.PackageTimestamp = obj.PackageUpdateTimestamp;
            info.Files = ConvertFiles(obj.Filenames);
            int stateValue;
            if(!Int32.TryParse(obj.PackageState, out stateValue))
            {
                stateValue = 0;
            }
            info.Status = ConvertStateToStatus(stateValue);

            info.State = obj.PackageState;
            return info;
        }

        private PackageStatus ConvertStateToStatus(int packageState)
        {
             if (packageState < 0)
             {
                return PackageStatus.ARCHIVED;

             }
             else if (packageState == 1 || packageState == 2 || packageState == 6)
             {
                 return PackageStatus.EXPIRED;
             }
             else
             {
                  return PackageStatus.ACTIVE;
             }
        }

        private List<File> ConvertFiles(List<string> files)
        {
            List<File> fileList = new List<File>();
            foreach (String fileName in files)
            {
                File f = new File();
                f.FileName = fileName;
                fileList.Add(f);
            }
            return fileList;
        }

        private List<Recipient> ConvertRecipients(List<string> recipients)
        {
            List<Recipient> recipientList = new List<Recipient>();
            if (recipients != null)
            {
                //Recipients are null for incoming (received) items
                foreach (String email in recipients)
                {
                    Recipient r = new Recipient();
                    r.Email = email;
                    recipientList.Add(r);
                }
            }
            return recipientList;
        }

        private List<PublicKeyRaw> GetPublicKeys(String packageId)
        {
            Endpoint p = ConnectionStrings.Endpoints["getPublicKeys"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);

            GetPublicKeysResponse response = connection.Send<GetPublicKeysResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return response.PublicKeys;
        }

        private EncryptedKeycode Create(PublicKeyRaw publicKey, String keycode)
        {
            CryptUtility cu = new CryptUtility();
            EncryptedKeycode encryptedKeycode = new EncryptedKeycode();
            encryptedKeycode.ID = publicKey.ID;
            encryptedKeycode.Keycode = cu.EncryptKeycode(publicKey.Key, keycode);
            return encryptedKeycode;
        }

        private void UploadKeycodes(String packageId, List<EncryptedKeycode> encryptedKeycodes)
        {
            foreach(EncryptedKeycode keycode in encryptedKeycodes)
            {
                Endpoint p = ConnectionStrings.Endpoints["addEncryptedKeycode"].Clone();
                p.Path = p.Path.Replace("{packageId}", packageId);
                p.Path = p.Path.Replace("{publicKeyId}", keycode.ID);

                UploadKeycodeRequest request = new UploadKeycodeRequest();
                request.Keycode = keycode.Keycode;

                StandardResponse response = connection.Send<StandardResponse>(p, request);
            }
        }

        private List<PackageInformation> Convert(List<PackageDTO> rawPackages)
        {
            List<PackageInformation> returnList = new List<PackageInformation>();
            foreach (PackageDTO raw in rawPackages)
            {
                returnList.Add(Convert(raw));
            }

            return returnList;
        }
        

        private Directory Convert(GetDirectoryResponse response)
        {
            Directory dir = new Directory(response.DirectoryId, response.DirectoryName);
            dir.SubDirectories = response.SubDirectories;
            dir.Files = response.Files;
            return dir;
        }

        private Recipient  Convert(AddRecipientResponse response)
        {
            Recipient rec = new Recipient();
            rec.Email = response.Email;
            rec.NeedsApproval = response.ApprovalRequired;
            rec.RecipientId = response.RecipientId;
            rec.RoleName = response.RoleName;
            return rec;
        }

        private List<RecipientHistory> Convert(List<RecipientHistoryDTO> rawPackages)
        {
            List<RecipientHistory> returnList = new List<RecipientHistory>();
            foreach (RecipientHistoryDTO raw in rawPackages)
            {
                returnList.Add(Convert(raw));
            }

            return returnList;
        }

        private PackageInformation Convert(PackageDTO raw)
        {
            PackageInformation packageInfo = new PackageInformation();
            packageInfo.PackageId = raw.PackageID;
            packageInfo.PackageCode = raw.PackageCode;
            packageInfo.ServerSecret = raw.ServerSecret;
            packageInfo.Approvers = raw.Approvers;
            packageInfo.PackageTimestamp = raw.PackageUpdateTimestamp;

            packageInfo.PackageOwner = raw.PackageUserName;
            
            packageInfo.Files = new List<File>();
            foreach (String fileName in raw.Filenames)
            {
                File f = new File();
                f.FileName = fileName;
                packageInfo.Files.Add(f);
            }

            packageInfo.NeedsApprover = raw.NeedsApprover;

            packageInfo.Recipients = new List<Recipient>();

            if (raw.Recipients != null)
            { 
                //Recipients are null for incoming (received) items
                foreach (String email in raw.Recipients)
                {
                    Recipient r = new Recipient();
                    r.Email = email;
                    packageInfo.Recipients.Add(r);
                }
            }

            packageInfo.Life = raw.Life;
            try
            {
                packageInfo.KeyCode = connection.GetKeycode(raw.PackageID);
            }
            catch (InvalidPackageException)
            {
                packageInfo.KeyCode = null;
            }

            return packageInfo;
        }

        private PackageInformation Convert(PackageInformationResponse raw)
        {
            PackageInformation packageInfo = new PackageInformation();
            packageInfo.PackageId = raw.PackageID;
            packageInfo.PackageCode = raw.PackageCode;
            packageInfo.ServerSecret = raw.ServerSecret;
            packageInfo.Approvers = raw.Approvers;
            packageInfo.PackageOwner = raw.PackageSender;
            packageInfo.PackageTimestamp = raw.PackageTimestamp; 
            packageInfo.Files = (raw.Files == null) ? new List<File>() : raw.Files;
            packageInfo.RootDirectoryId = raw.RootDirectoryId;

            packageInfo.NeedsApprover = raw.NeedsApprover;
            packageInfo.Recipients = (raw.Recipients == null) ? new List<Recipient>() : raw.Recipients;
            packageInfo.Life = raw.Life;
            packageInfo.PackageDescriptor = raw.Label;
            packageInfo.IsWorkspace = raw.IsVDR;
            packageInfo.State = raw.State;
            packageInfo.ContactGroups = raw.ContactGroups;
            int stateValue = (int) Enum.Parse(typeof(PackageState), raw.State, true);
            packageInfo.Status = ConvertStateToStatus(stateValue);

            try
            {
                packageInfo.KeyCode = connection.GetKeycode(raw.PackageID);
            }
            catch (InvalidPackageException)
            {
                packageInfo.KeyCode = null;
            }

            return packageInfo;
        }

        private FileInformation Convert(FileInformationResponse raw)
        {
            Objects.FileInformation fileInfo = new Objects.FileInformation();
            fileInfo = raw.File;
            return fileInfo;
        }

        private RecipientHistory Convert(RecipientHistoryDTO raw)
        {
            RecipientHistory recipientHistory = new RecipientHistory();
            recipientHistory.PackageCode = raw.PackageCode;
            recipientHistory.PackageContainsMessage = raw.PackageContainsMessage;
            recipientHistory.PackageID = raw.PackageID;
            recipientHistory.PackageLife = raw.PackageLife;
            recipientHistory.PackageOS = raw.PackageOS;
            recipientHistory.PackageRecipientResponse = raw.PackageRecipientResponse;
            recipientHistory.PackageState = raw.PackageState;
            recipientHistory.PackageStateColor = raw.PackageStateColor;
            recipientHistory.PackageStateStr = raw.PackageStateStr;
            recipientHistory.PackageUpdateTimestampStr = raw.PackageUpdateTimestampStr;
            recipientHistory.PackageUserId = raw.PackageUserId;
            recipientHistory.PackageUserName = raw.PackageUserName;
            recipientHistory.Files = (raw.Files == null) ? new List<String>() : raw.Files;
            return recipientHistory;
        }

        private String getKeyCode(String secureLink)
        {
            Uri myUri = new Uri(secureLink);
            return getKeyCode(myUri);
        }

        private String getKeyCode(Uri secureLink)
        {
            String fragment = secureLink.Fragment;

            String keyCode = fragment.Substring(fragment.IndexOf("=") + 1);

            if (keyCode == null)
            {
                throw new InvalidLinkException("The keycode could not be found in the Secure Link");
            }

            return keyCode;
        }

        private FileInfo createSegment(FileStream inputStream, long bytesToRead) 
        {
            System.IO.FileInfo segment = new System.IO.FileInfo(Path.GetTempFileName());

            long readBytes = 0;
            using (FileStream outStream = segment.OpenWrite())
            {
                byte[] buf = new byte[1 << 16];
                int len;
                int bufferBytesToRead = Math.Min(buf.Length, (int)(bytesToRead));
                while ((len = inputStream.Read(buf, 0, bufferBytesToRead)) > 0)
                {
                    outStream.Write(buf, 0, len);

                    readBytes += len;
                    bufferBytesToRead = Math.Min((int)(bytesToRead - readBytes), buf.Length);
                }
            }
            
            return segment;
        }

        private String createFileId(PackageInformation packageInfo, String filename, long filesize, int parts, String uploadType)
        {
            return createFileId(packageInfo, null, filename, filesize, parts,uploadType);
        }

        private String createFileId(PackageInformation packageInfo, String directoryId, String filename, long filesize, int parts, String uploadType)
        {
            Endpoint p = ConnectionStrings.Endpoints["createFileId"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);

            CreateFileIdRequest request = new CreateFileIdRequest();
            request.Filename = filename;
            request.Parts = parts;
            request.Filesize = filesize;
            request.DirectoryId = directoryId;

            request.UploadType = uploadType;
            StandardResponse response = connection.Send<StandardResponse>(p, request);

            if (response.Response == APIResponse.DENIED || response.Response == APIResponse.FAIL)
            {
                throw new FileUploadException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return response.Message;
        }

        private int calculateParts(long filesize)
        {
            if (filesize == 0)
            {
                return 1;
            }

            int parts;
            parts = (int)((filesize + SEGMENT_SIZE - 1) / SEGMENT_SIZE);

            Logger.Log("We need " + parts + " parts");
            return parts;
        }

        private void encryptAndUploadSegment(Endpoint p, PackageInformation packageInfo, int partIndex, System.IO.FileInfo unencryptedSegment, String filename, char[] passPhrase, long uploadedSoFar, long filesize, ISendSafelyProgress progress, String uploadType)
        {
            // Create a temp file to store the encrypted data in.
            System.IO.FileInfo encryptedData = new System.IO.FileInfo(Path.GetTempFileName());

            CryptUtility cu = new CryptUtility();
            cu.EncryptFile(encryptedData, unencryptedSegment, filename, passPhrase, progress);

            FileUploader fu = new FileUploader(connection, p, progress);
            //Logger.Log("File length: " + encryptedData.Length);

            connection.AddKeycode(packageInfo.PackageId, packageInfo.KeyCode);

            UploadFileRequest requestData = new UploadFileRequest();
            requestData.UploadType = uploadType;
            requestData.FilePart = partIndex;

            //StandardResponse response = fu.upload(encryptedData, filename, signature, encryptedData.Length, uploadType);
            StandardResponse response = fu.Upload(encryptedData, requestData, unencryptedSegment.Name, uploadedSoFar, filesize);
            encryptedData.Delete();
        }

        private String CreateEncryptionKey(String serverSecret, String keyCode)
        {
            return serverSecret + keyCode;
        }

        private void VerifyKeycode(String keycode)
        {
            if (keycode == null)
            {
                throw new MissingKeyCodeException("Keycode is null");
            }
            else if (keycode.Length == 0)
            {
                throw new MissingKeyCodeException("Keycode is empty");
            }
            else if (keycode.Length < 32)
            {
                throw new MissingKeyCodeException("Keycode is to short");
            }
        }

        private String getPackageCode(String link)
        {
            return getParameterFromURL(new Uri(link), "packageCode");
        }

        private String getPackageCode(Uri link)
        {
            return getParameterFromURL(link, "packageCode");
        }

        private String getParameterFromURL(Uri link, String parameter)
        {
            String packageCode = System.Web.HttpUtility.ParseQueryString(link.Query).Get(parameter);

            if (packageCode == null)
            {
                throw new InvalidLinkException("Package code could not be found");
            }

            return packageCode;
        }

        private List<Recipient> convert(List<RecipientResponse> recipients)
        {
            List<Recipient> convertedRecipients = new List<Recipient>();
            foreach(RecipientResponse item in recipients)
            {
                convertedRecipients.Add(convert(item));
            }

            return convertedRecipients;
        }

        private Recipient convert(RecipientResponse item)
        {
            Recipient recipient = new Recipient();
            recipient.Email = item.Email;
            recipient.RecipientId = item.RecipientId;
            recipient.NeedsApproval = item.NeedsApproval;
            return recipient;
        }

        private Recipient convert(GetRecipientResponse response)
        {
            Recipient recipient = new Recipient();
            recipient.Email = response.Email;
            recipient.NeedsApproval = response.ApprovalRequired;
            recipient.RecipientId = response.RecipientId;
            return recipient;
        }
        #endregion
    }
}
