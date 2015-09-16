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
            Endpoint p = ConnectionStrings.Endpoints["createPackage"].Clone();
            CreatePackageResponse response = connection.Send<CreatePackageResponse>(p);

            Logger.Log("Response: " + response.Response);
            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            // Derive keycode
            CryptUtility cu = new CryptUtility();

            PackageInformation packageInfo = new PackageInformation();
            packageInfo.PackageId = response.PackageId;
            packageInfo.KeyCode = cu.GenerateToken();
            packageInfo.PackageCode = response.PackageCode;
            packageInfo.ServerSecret = response.ServerSecret;

            Logger.Log("Adding keycode to package: " + packageInfo.PackageId);
            connection.AddKeycode(packageInfo.PackageId, packageInfo.KeyCode);

            return packageInfo;
        }

        public List<PackageInformation> GetActivePackages()
        {
            Endpoint p = ConnectionStrings.Endpoints["activePackages"].Clone();
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

        public Recipient AddRecipient(String packageId, String email)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            Logger.Log("Attempting to add " + email + " to " + packageId);

            Endpoint p = ConnectionStrings.Endpoints["addRecipient"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageId);

            AddRecipientRequest request = new AddRecipientRequest();
            request.Email = email;
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
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            connection.AddKeycode(packageId, keycode);

            // Get the updated package information.
            PackageInformation packageInfo = GetPackageInformation(packageId);
            packageInfo.KeyCode = keycode;
            return EncryptAndUploadFile(packageInfo, path, progress, uploadType);
        }

        public File EncryptAndUploadFile(PackageInformation packageInfo, String path, ISendSafelyProgress progress, String uploadType)
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
            FileInfo unenCryptedFile = new FileInfo(path);
            String filename = unenCryptedFile.Name;

            int parts = calculateParts(unenCryptedFile.Length);
            String fileId = createFileId(packageInfo, filename, unenCryptedFile.Length, parts, uploadType);

            Endpoint p = ConnectionStrings.Endpoints["uploadFile"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);
            p.Path = p.Path.Replace("{fileId}", fileId);

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
                    FileInfo segment = createSegment(readStream, (partSize + offset));
                    encryptAndUploadSegment(p, packageInfo, i, segment, filename, passPhrase, uploadedSoFar, approximateFilesize, progress, uploadType);
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

        public FileInfo DownloadFile(String packageId, String fileId, String keycode, ISendSafelyProgress progress, String downloadAPI)
        {
            if (packageId == null)
            {
                throw new InvalidPackageException("Package ID can not be null");
            }

            connection.AddKeycode(packageId, keycode);

            // Get the updated package information.
            PackageInformation packageInfo = GetPackageInformation(packageId);
            packageInfo.KeyCode = keycode;
            return DownloadFile(packageInfo, fileId, progress, downloadAPI);
        }

        public FileInfo DownloadFile(PackageInformation pkgInfo, String fileId, ISendSafelyProgress progress, String downloadAPI)
        {
            DownloadFileUtility downloadUtility = new DownloadFileUtility(connection, pkgInfo, progress, downloadAPI);
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
            if (packageInfo == null)
            {
                throw new InvalidPackageException("PackageInformation can not be null");
            }

            VerifyKeycode(packageInfo.KeyCode);

            Endpoint p = ConnectionStrings.Endpoints["finalizePackage"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);

            FinalizePackageRequest request = new FinalizePackageRequest();
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

        #endregion

        #region Private Functions

        private List<PackageInformation> Convert(List<PackageDTO> rawPackages)
        {
            List<PackageInformation> returnList = new List<PackageInformation>();
            foreach (PackageDTO raw in rawPackages)
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

            packageInfo.Files = new List<File>();
            foreach (String fileName in raw.Files)
            {
                File f = new File();
                f.FileName = fileName;
                packageInfo.Files.Add(f);
            }

            packageInfo.NeedsApprover = raw.NeedsApprover;

            packageInfo.Recipients = new List<Recipient>();
            foreach (String email in raw.Recipients)
            {
                Recipient r = new Recipient();
                r.Email = email;
                packageInfo.Recipients.Add(r);
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

            packageInfo.Files = (raw.Files == null) ? new List<File>() : raw.Files;

            packageInfo.NeedsApprover = raw.NeedsApprover;
            packageInfo.Recipients = (raw.Recipients == null) ? new List<Recipient>() : raw.Recipients;
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
            FileInfo segment = new FileInfo(Path.GetTempFileName());

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
            Endpoint p = ConnectionStrings.Endpoints["createFileId"].Clone();
            p.Path = p.Path.Replace("{packageId}", packageInfo.PackageId);

            CreateFileIdRequest request = new CreateFileIdRequest();
            request.Filename = filename;
            request.Parts = parts;
            request.Filesize = filesize;
            
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

        private void encryptAndUploadSegment(Endpoint p, PackageInformation packageInfo, int partIndex, FileInfo unencryptedSegment, String filename, char[] passPhrase, long uploadedSoFar, long filesize, ISendSafelyProgress progress, String uploadType)
        {
            // Create a temp file to store the encrypted data in.
            FileInfo encryptedData = new FileInfo(Path.GetTempFileName());

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

        #endregion

    }
}
