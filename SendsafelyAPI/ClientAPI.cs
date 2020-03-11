using System;
using System.Collections.Generic;
using System.Text;
using SendSafely.Objects;
using SendSafely.Utilities;
using SendSafely.Exceptions;
using System.Net;
using System.IO;

namespace SendSafely
{
    /// <summary>
    /// The front facing class of the API. All interaction with SendSafely is done through this class
    /// </summary>
    public class ClientAPI
    {
        private Connection connection = null;

        /// <summary>
        /// Initializes the API. This function must be called before the API can be used.
        /// </summary>
        /// <param name="host"> The hostname you use to access SendSafely. Should be https://companyname.sendsafely.com or https://www.sendsafely.com</param>
        /// <param name="apiKey"> The API key for the user. A new API key can be generated on the user's Edit Profile page of the SendSafely web portal or using the generateAPIKey API method. </param>
        /// <param name="apiSecret"> The secret belonging to the API key. </param>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        public void InitialSetup(string host, string apiKey, string apiSecret)
        {
            InitialSetup(host, apiKey, apiSecret, (WebProxy)null);
        }

        /// <summary>
        /// Initializes the API. This function must be called before the API can be used.
        /// </summary>
        /// <param name="host"> The hostname you use to access SendSafely. Should be https://companyname.sendsafely.com or https://www.sendsafely.com</param>
        /// <param name="apiKey"> The API key for the user. A new API key can be generated on the user's Edit Profile page of the SendSafely web portal or using the generateAPIKey API method. </param>
        /// <param name="apiSecret"> The secret belonging to the API key. </param>
        /// <param name="proxy"> A web proxy object which already contains proxy information.</param>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        public void InitialSetup(string host, string apiKey, string apiSecret, WebProxy proxy)
        {
            StartupUtility su = new StartupUtility(host, apiSecret, apiKey, proxy);
            this.connection = su.GetConnectionObject();
        }

        /// <summary>
        /// Initializes the API. This function must be called before the API can be used.
        /// </summary>
        /// <param name="host"> The hostname you use to access SendSafely. Should be https://companyname.sendsafely.com or https://www.sendsafely.com</param>
        /// <param name="proxy"> A web proxy object which already contains proxy information.</param>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        public void InitialSetup(string host, WebProxy proxy)
        {
            this.connection = new Connection(host, proxy);
        }

        /// <summary>
        /// Initializes the API. This function must be called before the API can be used. 
        /// </summary>
        /// <param name="host"> The hostname you use to access SendSafely. Should be https://companyname.sendsafely.com or https://www.sendsafely.com</param>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        public void InitialSetup(string host)
        {
            InitialSetup(host, (WebProxy)null);
        }

        /// <summary>
        /// Add Contact Group as a recipient on a package.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the add the Contact Group operation.</param>
        /// <param name="groupId"> The unique id of the Contact Group to add to the package.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void AddContactGroupToPackage(String packageId, String groupId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.AddContactGroupToPackage(packageId, groupId);
        }

        /// <summary>
        /// Adds a recipient email address to the current user's Dropzone.
        /// </summary>
        /// <param name="email"> The recipient email address added to the Dropzone.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void AddDropzoneRecipient(String email)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.AddDropzoneRecipient(email);
        }

        /// <summary>
        /// Adds a recipient to a given package.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for get directory operation.</param>
        /// <param name="email"> The recipient email to be added.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="InvalidEmailException">Thrown when the given email is incorrect.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        /// <returns>
        /// Returns a recipient object describing the newly added Recipient.
        /// </returns>
        public Recipient AddRecipient(String packageId, String email)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.AddRecipient(packageId, email);
        }

        /// <summary>
        /// Adds a phone number to a given recipient.
        /// </summary>
        /// <param name="packageId"> The unique packageId that you are updating.</param>
        /// <param name="recipientId"> The recipientId to be updated</param>
        /// <param name="phoneNumber"> The phone number to associate with the recipient. Passing a phone number with a numeric country code prefix (i.e. +44), will effectively override the countryCode parameter.</param>
        /// <param name="countryCode"> The country code that belongs to the phone number.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="InvalidPhonenumberException">Thrown when the phonenumber or country code is incorrect.</exception>
        /// <exception cref="InvalidRecipientException">Thrown when the recipientId is invalid.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void AddRecipientPhoneNumber(String packageId, String recipientId, String phoneNumber, CountryCodes.CountryCode countryCode)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.AddRecipientPhonenumber(packageId, recipientId, phoneNumber, countryCode);
        }

        /// <summary>
        /// Adds a list of recipients to the package.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the add recipient operation.</param>
        /// <param name="emails">The list of recipients to be added.</param>
        /// <exception cref="LimitExceededException">Will be thrown if the server returns a limit exceeded message.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        /// <returns>
        /// Returns the list containing information about the recipients.
        /// </returns>
        public List<Recipient> AddRecipients(String packageId, List<String> emails)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.AddRecipients(packageId, emails);
        }

        /// <summary>
        /// Add user email address to the specified Contact Group.  
        /// </summary>
        /// <param name="groupId"> The unique id of the Contact Group for the add user operation.</param>
        /// <param name="userEmail"> The email address to add to the Contact Group. </param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// The unique id of the user associated with the email address added to the Contact Group.
        /// </returns>
        public String AddUserToContactGroup(String groupId, String userEmail)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.AddGroupUser(groupId, userEmail);
        }

        /// <summary>
        /// Create a new Contact Group with the passed in group name. A Contact Group allows a user to define and manage a group of recipients at the group-level, rather than individually on each package. For more information about Contact Groups, refer to http://sendsafely.github.io/overview.htm 	 
        /// </summary>
        /// <param name="groupName">The name of the new Contact Group.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// The unique group id of the created Contact Group. This value is required for subsequent Contact Group management operations.  
        /// </returns>
        public String CreateContactGroup(String groupName)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.CreateContactGroup(groupName);

        }

        /// <summary>
        /// Create a new Enterprise Contact Group with the passed in group name. The method caller must be a SendSafely Enterprise Administrator, and the Contact Group it creates is available to all users in an organization. For more information on Contact Groups, refer to http://sendsafely.github.io/overview.htm
        /// </summary>
        /// <param name="groupName">The name of the new Contact Group.</param>
        /// <param name="isEnterpriseGroup">A boolean flag for determining whether a Contact Group is an Enterprise Contact Group. If set to true, subsequent management operations of the Contact Group will require SendSafely Enterprise Administrator privileges, however the Contact Group can be added as a recipient to any package by any user in the organization. </param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// The unique group id of the created Contact Group. This value is required for subsequent Contact Group management operations.  
        /// </returns>
        public String CreateContactGroup(String groupName, Boolean isEnterpriseGroup)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.CreateContactGroup(groupName, Convert.ToString(isEnterpriseGroup));

        }

        /// <summary>
        /// Creates a new directory in a Workspace. Only Workspace packages support directories.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the create directory operation.</param>
        /// <param name="parentDirectoryId">The unique id of the created directory's parent directory. If creating a directory in the root directory of a Workspace, this will be the packageDirectoryId property of the Workspace Package object. Otherwise, this will be the directoryId property of the parent Directory object. </param>
        /// <param name="directoryName">The name of the new directory to be created.</param>
        /// <returns>A Directory object containing information about the created directory.</returns>
        public Directory CreateDirectory(String packageId, String parentDirectoryId, String directoryName)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            String directoryId = pu.CreateDirectory(packageId, parentDirectoryId, directoryName);
            return this.GetDirectory(packageId, directoryId);
        }

        /// <summary>
        ///  Creates a new package for the purpose of sending files. A new package must be created before files or recipients can be added. For further information about the package flow, see http://sendsafely.github.io/overview.htm
        /// </summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="LimitExceededException">Thrown when the limits for the user has been exceeded.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        /// <returns>
        /// A PackageInfo object containing information about the package.
        /// </returns>
        public PackageInformation CreatePackage()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.CreatePackage();
        }

        /// <summary>
        /// Creates a new package that represents a secure Workspace. A Workspace is a type of package that supports file collaboration features, such as directories and subdirectories, file versioning, role-based access control, and activity logging. A Workspace must be created before files, directories, or collaborators can be added. For further information about the package flow and Workspaces, refer to http://sendsafely.github.io/overview.htm
        /// </summary>
        /// <param name="isWorkspace">Flag declaring the package is a Workspace.</param>
        /// <returns>Returns the package information object on the newly created package.</returns>
        public PackageInformation CreatePackage(bool isWorkspace)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.CreatePackage(isWorkspace);
        }

        /// <summary>
        /// Creates a new package and assigns package owner to the user whose email address is passed as the method argument. The method caller must be a SendSafely Enterprise Administrator and in the same organization as the assigned package owner.   
        /// </summary>
        /// <param name="email"> The email address of the package owner.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="LimitExceededException">Thrown when the limits for the user has been exceeded.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        /// <returns>
        /// A PackageInfo object containing information about the package.
        /// </returns>
        public PackageInformation CreatePackageForUser(String email)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.CreatePackage(email);
        }

        /// <summary>
        /// Delete the Contact Group associated with the passed in group id. 
        /// </summary>
        /// <param name="groupId"> The unique id of the Contact Group to delete.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void DeleteContactGroup(String groupId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.DeleteContactGroup(groupId);
        }

        /// <summary>
        /// Deletes a directory from a Workspace package.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the delete directory operation.</param>
        /// <param name="directoryId">The unique directory id of the directory to delete.</param>
        public void DeleteDirectory(String packageId, String directoryId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.DeleteDirectory(packageId, directoryId);
        }

        /// <summary>
        /// Deletes a file from a Workspace package.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the delete file operation.</param>
        /// <param name="directoryId">The unique directory id of the directory containing the file to delete.</param>
        /// <param name="fileId">The unique file id of the file to delete.</param>
        public void DeleteFile(String packageId, String directoryId, String fileId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.DeleteFile(packageId, directoryId, fileId);
        }

        /// <summary>
        /// Deletes a package with the given package id.
        /// <seealso cref="CreatePackage()">CreatePackage()</seealso>.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package to be deleted.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void DeletePackage(String packageId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.DeletePackage(packageId);
        }

        /// <summary>
        /// Deletes a temporary package, which is a package that has not yet been finalized. 
        /// </summary>
        /// <param name="packageId"> The unique package id of the package to be deleted.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void DeleteTempPackage(String packageId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.DeleteTempPackage(packageId);
        }

        /// <summary>
        /// Downloads a file from the server and decrypts it.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the file download operation.</param>
        /// <param name="fileId">The unique file id of the file to download.</param>
        /// <param name="keyCode">The keycode belonging to the package. </param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the download is progressing.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A FileInfo object containing a temporary file name. The file must be renamed to be usable through any program using this function.
        /// </returns>
        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, ISendSafelyProgress progress)
        {
            return DownloadFile(packageId, fileId, keyCode, progress, "CSHARP");
        }

        /// <summary>
        /// Downloads a file from the server and decrypts it.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the file download operation.</param>
        /// <param name="fileId">The unique file id of the file to download.</param>
        /// <param name="keyCode">The keycode belonging to the package.</param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the download is progressing.</param>
        /// <param name="downloadAPI">A String identifying the API which is downloading the file. Defaults to "CSHARP".</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A FileInfo object containing a temporary file name. The file must be renamed to be usable through any program using this function.
        /// </returns>
        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, ISendSafelyProgress progress, String downloadAPI)
        {
            return DownloadFile(packageId, fileId, keyCode, progress, downloadAPI, null);
        }

        /// <summary>
        /// Downloads a file from the server and decrypts it.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the file download operation.</param>
        /// <param name="fileId">The unique file id of the file to download.</param>
        /// <param name="keyCode">The keycode belonging to the package.</param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the download is progressing.</param>
        /// <param name="downloadAPI">A String identifying the API which is downloading the file. Defaults to "CSHARP".</param>
        /// <param name="password">The password required to download a file from a password protected undisclosed package (i.e. a package without any recipients assigned).</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A FileInfo object containing a temporary file name. The file must be renamed to be usable through any program using this function.
        /// </returns>
        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, ISendSafelyProgress progress, String downloadAPI, String password)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.DownloadFile(packageId, null, fileId, keyCode, progress, downloadAPI, password);
        }

        /// <summary>
        /// Downloads a file from the server and decrypts it.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the file download operation.</param>
        /// <param name="fileId">The unique file id of the file to download.</param>
        /// <param name="keyCode">The keycode belonging to the package.</param>
        /// <param name="password">The password required to download a file from a password protected undisclosed package (i.e. a package without any recipients assigned).</param>
        /// <returns>A FileInfo object containing a temporary file name. The file must be renamed to be usable through any program using this function.</returns>
        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, String password)
        {
            return DownloadFile(packageId, fileId, keyCode, null, "CSHARP", password);
        }

        /// <summary>
        /// Downloads a file from the server and decrypts it.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the file download operation.</param>
        /// <param name="fileId">The unique file id of the file to download.</param>
        /// <param name="keyCode">The keycode belonging to the package.</param>
        /// <param name="password">The password required to download a file from a password protected undisclosed package (i.e. a package without any recipients assigned).</param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the download is progressing.</param>
        /// <returns>A FileInfo object containing a temporary file name. The file must be renamed to be usable through any program using this function.</returns>
        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, String password, ISendSafelyProgress progress)
        {
            return DownloadFile(packageId, fileId, keyCode, progress, "CSHARP", password);
        }

        /// <summary>
        /// Downloads a file located in a directory of a Workspace package from the server and decrypts it.
        /// </summary>
        /// <param name="packageId">The unique package id of the Workspace package for the file download operation.</param>
        /// <param name="directoryId">The unique directory id of the directory for the file download operation. </param>
        /// <param name="fileId">The unique file id of the file to download.</param>
        /// <param name="keyCode">The keycode belonging to the package.</param>
        /// <param name="progress"> A progress callback object which can be used to report back progress on how the upload is progressing.</param>
        /// <returns>System FileInfo Object containing a temporary file name. The file must be renamed to be usable through any program using this function.</returns>
        public FileInfo DownloadFileFromDirectory(String packageId, String directoryId, String fileId, String keyCode, ISendSafelyProgress progress)
        {
            return DownloadFileFromDirectory(packageId, directoryId, fileId, keyCode, progress, "CSHARP");
        }

        /// <summary>
        /// Downloads a file located in a directory of a Workspace package from the server and decrypts it.
        /// </summary>
        /// <param name="packageId">The unique package id of the Workspace package for the file download operation.</param>
        /// <param name="directoryId">The unique directory id of the directory for the file download operation.</param>
        /// <param name="fileId">The unique file id of the file to download.</param>
        /// <param name="keyCode">The keycode belonging to the package.</param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the upload is progressing.</param>
        /// <param name="api">The api value.</param>
        /// <returns>A FileInfo object containing a temporary file name. The file must be renamed to be usable through any program using this function.</returns>
        public FileInfo DownloadFileFromDirectory(String packageId, String directoryId, String fileId, String keyCode, ISendSafelyProgress progress, String api)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.DownloadFile(packageId, directoryId, fileId, keyCode, progress, api, null);
        }

        /// <summary>
        /// Encrypt and upload a new file. The file will be encrypted before being uploaded to the server. The function will block until the file is uploaded.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the file upload operation. </param>
        /// <param name="path">The path to the file.</param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the upload is progressing.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="FileUploadException">Thrown when a file segment failed to upload a couple of times. Will usually happen when the internet connection is lost or very weak.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns a file object referencing the file.
        /// </returns>
        public File EncryptAndUploadFile(String packageId, String path, ISendSafelyProgress progress)
        {
            EnforceInitialized();

            String keyCode;
            try
            {
                keyCode = connection.GetKeycode(packageId);
            }
            catch (InvalidPackageException e)
            {
                if (packageId == null)
                {
                    throw e;
                }
                throw new MissingKeyCodeException();
            }

            return EncryptAndUploadFile(packageId, keyCode, path, progress, "CSHARP");
        }

        /// <summary>
        /// Encrypt and upload a new file. The file will be encrypted before being uploaded to the server. The function will block until the file is uploaded.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the file upload operation.  </param>
        /// <param name="keyCode">The keycode belonging to the package. </param>
        /// <param name="path">The path to the file.</param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the upload is progressing.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="FileUploadException">Thrown when a file segment failed to upload a couple of times. Will usually happen when the internet connection is lost or very weak.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns a file object referencing the file.
        /// </returns>
        public File EncryptAndUploadFile(String packageId, String keyCode, String path, ISendSafelyProgress progress)
        {
            EnforceInitialized();

            return EncryptAndUploadFile(packageId, keyCode, path, progress, "CSHARP");
        }

        /// <summary>
        /// Encrypt and upload a new file. The file will be encrypted before being uploaded to the server. The function will block until the file is uploaded.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the file upload operation. </param>
        /// <param name="keyCode">The keycode belonging to the package. </param>
        /// <param name="path">The path to the file.</param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the upload is progressing.</param>
        /// <param name="uploadType">The upload type.</param>
        /// <returns>Returns a file object referencing the file.</returns>
        public File EncryptAndUploadFile(String packageId, String keyCode, String path, ISendSafelyProgress progress, String uploadType)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);

            File file = pu.EncryptAndUploadFile(packageId, keyCode, path, progress, uploadType);
            return file;
        }

        /// <summary>
        /// Encrypt and upload a new file to a directory in a Workspace package. The file will be encrypted before being uploaded to the server. The function will block until the file is uploaded.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the file upload operation. </param>
        /// <param name="directoryId">The unique directory id of the directory for the file upload operation.  </param>
        /// <param name="keyCode">The keycode belonging to the package.</param>
        /// <param name="path">The path to the file to upload</param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the upload is progressing.</param>
        /// <returns>Returns the newly uploaded file object</returns>
        public File EncryptAndUploadFileInDirectory(String packageId, String directoryId, String keyCode, String path, ISendSafelyProgress progress)
        {
            EnforceInitialized();

            return EncryptAndUploadFileInDirectory(packageId, directoryId, keyCode, path, progress, "CSHARP");
        }

        /// <summary>
        /// Encrypt and upload a new file to a directory in a Workspace package. The file will be encrypted before being uploaded to the server. The function will block until the file is uploaded.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the file upload operation. </param>
        /// <param name="directoryId">The unique directory id of the directory for the file upload operation.</param>
        /// <param name="keyCode">The keycode belonging to the package.</param>
        /// <param name="path">The path to the file to upload</param>
        /// <param name="progress">A progress callback object which can be used to report back progress on how the upload is progressing.</param>
        /// <param name="uploadType">The upload type.</param>
        /// <returns>Returns the newly uploaded file object</returns>
        public File EncryptAndUploadFileInDirectory(String packageId, String directoryId, String keyCode, String path, ISendSafelyProgress progress, String uploadType)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);

            File file = pu.EncryptAndUploadFile(packageId, directoryId, keyCode, path, progress, uploadType);
            return file;
        }

        /// <summary>
        /// Encrypt and upload a message. The message will be encrypted before being uploaded to the server.
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the message upload operation.</param>
        /// <param name="message">The message string to encrypt and upload.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void EncryptAndUploadMessage(String packageId, String message)
        {
            EnforceInitialized();

            Logger.Log("Encrypting to packageID: " + packageId);
            String keyCode;
            try
            {
                keyCode = connection.GetKeycode(packageId);
            }
            catch (InvalidPackageException e)
            {
                if (packageId == null)
                {
                    throw e;
                }
                throw new MissingKeyCodeException();
            }

            EncryptAndUploadMessage(packageId, keyCode, message, "CSHARP");
        }

        /// <summary>
        /// Encrypt and upload a message. The message will be encrypted before being uploaded to the server.
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the message upload operation.</param>
        /// <param name="keyCode">The keycode belonging to the package. </param>
        /// <param name="message">The message to encrypt and upload.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void EncryptAndUploadMessage(String packageId, String keyCode, String message)
        {
            EnforceInitialized();

            EncryptAndUploadMessage(packageId, keyCode, message, "CSHARP");
        }

        /// <summary>
        /// Encrypt and upload a message. The message will be encrypted before being uploaded to the server.
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the message upload operation.</param>
        /// <param name="keyCode">The keycode belonging to the package. </param>
        /// <param name="message">The message to encrypt and upload.</param>
        /// <param name="uploadType">The upload type.</param>
        public void EncryptAndUploadMessage(String packageId, String keyCode, String message, String uploadType)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);

            pu.EncryptAndUploadMessage(packageId, keyCode, message, uploadType);
        }

        /// <summary>
        /// Finalizes the package so it can be delivered to the recipients.
        /// <seealso cref="CreatePackage()">createPackage()</seealso>. Additionally, the package must contain at least one file.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package to be finalized.</param>
        /// <param name="keycode"> The keycode belonging to the package.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="PackageFinalizationException">Thrown when the package couldn't be finalized. The message will contain detailed information</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        ///  A link to access the package. This link can be sent to the recipients.
        /// </returns>
        public String FinalizePackage(String packageId, String keycode)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.FinalizePackage(packageId, keycode);
        }

        /// <summary>
        /// Finalizes the package so it can be delivered to the recipients.
        /// <seealso cref="CreatePackage()">createPackage()</seealso>. Additionally, the package must contain at least one file.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package to be finalized.</param>
        /// <param name="keycode"> The keycode belonging to the package.</param>
        /// <param name="allowReplyAll"> Determines whether package recipients permitted to reply to all recipients or just sender.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="PackageFinalizationException">Thrown when the package couldn't be finalized. The message will contain detailed information</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        ///  A link to access the package. This link can be sent to the recipients.
        /// </returns>
        public String FinalizePackage(String packageId, String keycode, bool allowReplyAll)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.FinalizePackage(packageId, keycode, allowReplyAll);
        }

        /// <summary>
        /// Finalizes an undisclosed package, which is a package without recipients. Anyone with access to the link can access the package. 
        /// </summary>
        /// <param name="packageId"> The unique package id of the package to be finalized.</param>
        /// <param name="keycode"> The keycode belonging to the package.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="PackageFinalizationException">Thrown when the package couldn't be finalized. The message will contain detailed information</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A link to access the package. This link can be sent to the recipients.
        /// </returns>
        public String FinalizeUndisclosedPackage(String packageId, String keycode)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.FinalizeUndisclosedPackage(packageId, keycode);
        }

        /// <summary>
        /// Finalizes an undisclosed package, which is a package without recipients, and protects it with a password. Anyone with access to the link will also be required to supply the password to access the package. 
        /// </summary>
        /// <param name="packageId"> The unique package id of the package to be finalized.</param>
        /// <param name="password"> A password that will be required to access the contents of the package.</param>
        /// <param name="keycode"> The keycode belonging to the package.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="PackageFinalizationException">Thrown when the package couldn't be finalized. The message will contain detailed information</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A link to access the package. This link can be sent to the recipients.
        /// </returns>
        public String FinalizeUndisclosedPackage(String packageId, String password, String keycode)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.FinalizeUndisclosedPackage(packageId, password, keycode);
        }

        /// <summary>
        /// Generates a new SendSafely API key and secret for the provided SendSafely user name and password. This key and secret can be used to authenticate to the SendSafely API. If the current user has Two-Step Authentication enabled, a TwoFactorAuthException exception will be returned to the client that includes a validation token. Additionally, a verification code will be sent via SMS message to the user's mobile number. Both the validation token and verification code will be needed in a follow-up call to the GenerateKey2FA API method in order to complete the authentication process and receive an API key and secret. 
        /// </summary>
        /// <param name="username">Email address of the user required for authenticating to SendSafely. </param>
        /// <param name="password">Password of the user required for authenticating to SendSafely.</param>
        /// <param name="keyDescription">User defined description of the generated API key.</param>
        /// <exception cref="TwoFactorAuthException">Thrown when two factor authentication is required. The exception contains a ValidationToken that must be used in the subsequent request.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the username/password combination is wrong.</exception>
        /// <exception cref="ActionFailedException">Thrown if any other error occurs.</exception>
        /// <returns>A APICredential object containing the api key.</returns>
        public APICredential GenerateAPIKey(String username, String password, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.GenerateAPIKey(username, password, keyDescription);
        }

        /// <summary>
        /// Generate a new API Key given a validation token and a SMS Code. This key and secret can be used to authenticate to the SendSafely API. If the current user has Two-Step Authentication enabled, a TwoFactorAuthException exception will be returned to the client that includes a validation token. Additionally, a verification code will be sent via SMS message to the user's mobile number. Both the validation token and verification code will be needed in a follow-up call to the GenerateKey2FA API method in order to complete the authentication process and receive an API key and secret. 
        /// </summary>
        /// <param name="validationLink">The validation token returned from a call to generateAPIKey() by a Two-Step Authentication enabled user. </param>
        /// <param name="smsCode">The SMS verification code received from a call to generateAPIKey() by a Two-Step Authentication enabled user. </param>
        /// <param name="keyDescription">User defined description of the generated API key.</param>
        /// <exception cref="InvalidCredentialsException">Thrown if the SMS Code is incorrect.</exception>
        /// <exception cref="PINRefreshException">Thrown if there's been to many failed attempts and a new SMS code is needed.</exception>
        /// <exception cref="ActionFailedException">Thrown if any other error occured.</exception>
        public APICredential GenerateKey2FA(String validationLink, String smsCode, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.GenerateAPIKey2FA(validationLink, smsCode, keyDescription);
        }

        /// <summary>
        /// Generates a new RSA Key pair used to encrypt keycodes. The private key as well as an identifier associating the public Key is returned to the user. 
        /// The public key is uploaded and stored on the SendSafely servers.
        /// </summary>
        /// <param name="description">The description used for generating the key pair.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        /// <returns>
        /// Returns a Private Key containing the armored private key and a Public Key ID associating a public key to the private key.
        /// </returns>
        public PrivateKey GenerateKeyPair(String description)
        {
            EnforceInitialized();

            PublicKeyUtility utility = new PublicKeyUtility(this.connection);
            return utility.GenerateKeyPair(description);
        }

        /// <summary>
        /// Get all active packages for the current user.
        /// </summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="LimitExceededException">Thrown when the limits for the user has been exceeded.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A List containing package metadata for all active packages.
        /// </returns>
        public List<PackageInformation> GetActivePackages()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetActivePackages();
        }

        /// <summary>
        /// Retrieves activity log records for a Workspace package. The method supports returning up to 10 records at a time. The caller must be the owner of the Workspace, assigned to the manager role within the Workspace, or an enterprise administrator.
        /// </summary>
        /// <param name="packageId">The unique package id of the Workspace package for the get activity log operation.</param>
        /// <param name="rowIndex">Integer representing the index of the first activity log record to retrieve. </param>
        /// <returns>List<ActivityLogEntry> object containing the activity log entries.</returns>
        public List<ActivityLogEntry> GetActivityLog(String packageId, int rowIndex)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetActivityLog(packageId, rowIndex);
        }

        /// <summary>
        /// Get all archived packages for the current user.
        /// </summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="LimitExceededException">Thrown when the limits for the user has been exceeded.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A List containing package metadata for all archived packages.
        /// </returns>
        public List<PackageInformation> GetArchivedPackages()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetArchivedPackages();
        }

        /// <summary>
        /// Retrieve the list of available Contact Groups for the current user profile, including all email addresses associated with each Contact Group. 
        /// </summary>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// List<ContactGroup> object of Contact Groups and email addresses.
        /// </returns>
        public List<ContactGroup> GetContactGroups()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetContactGroups(false);
        }

        [Obsolete("getContactGroups is deprecated, please use GetContactGroups instead", false)]
        public List<ContactGroup> getContactGroups()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetContactGroups(false);
        }

        /// <summary>
        /// Retrieves meta data about a directory in a Workspace package.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the target directory.</param>
        /// <param name="directoryId">The unique directory id of the target directory. </param>
        /// <returns>A Directory object containing information about the directory.</returns>
        public Directory GetDirectory(String packageId, String directoryId)
        {
            EnforceInitialized();
            PackageUtility pu = new PackageUtility(connection);
            return pu.GetDirectory(packageId, directoryId);

        }

        /// <summary>
        /// Gets all recipients assigned to the current user's Dropzone.
        /// </summary>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message as we expect the server to return a success.</exception>
        /// <returns>
        /// Returns the list of dropzone recipient email addresses that are Dropzone recipients.
        /// </returns>
        public List<String> GetDropzoneRecipients()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetDropzoneRecipients();
        }

        /// <summary>
        /// Retrieve the list of available Contact Groups in the user's organization, including all email addresses associated with each Contact Group.
        /// </summary>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// List<ContactGroup> object of Contact Groups and email addresses.
        /// </returns>
        public List<ContactGroup> GetEnterpriseContactGroups()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetContactGroups(true);
        }

        [Obsolete("getEnterpriseContactGroups is deprecated, please use GetEnterpriseContactGroups instead", false)]
        public List<ContactGroup> getEnterpriseContactGroups()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetContactGroups(true);
        }

        /// <summary>
        /// Retrieves information about the organization the user belongs to. 
        /// </summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        /// <returns>
        /// Returns an object containing the retrieved information.
        /// </returns>
        public EnterpriseInformation GetEnterpriseInfo()
        {
            EnforceInitialized();

            EnterpriseUtility util = new EnterpriseUtility(connection);
            return util.GetInformation();
        }

        /// <summary>
        /// Retrieves meta data about a file in a Workspace package.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the get file information operation.</param>
        /// <param name="directoryId">The unique directory id of the directory containing the target file.</param>
        /// <param name="fileId">The unique file id of the target file.</param>
        /// <returns>A FileInformation object containing information about the file.</returns>
        public FileInformation GetFileInformation(String packageId, String directoryId, String fileId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);

            return pu.GetFileInformation(packageId, directoryId, fileId);
        }

        /// <summary>
        /// Downloads and decrypts a keycode from the server for a given packageId and RSA Key pair. 
        /// </summary>
        /// <param name="privateKey">The private key associated with the RSA Key pair used to encrypt the package keycode.</param>
        /// <param name="packageId">The package id for the keycode.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="GettingKeycodeFailedException">Will be thrown if the server returns an error message while downloading the keycode.</exception>
        /// <returns>
        /// Returns the decrypted keycode.
        /// </returns>
        public String GetKeycode(PrivateKey privateKey, String packageId)
        {
            EnforceInitialized();

            PublicKeyUtility utility = new PublicKeyUtility(this.connection);
            return utility.GetKeycode(packageId, privateKey);
        }

        /// <summary>
        /// Downloads a message from the specified secure link and decrypts it.
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.
        /// </summary>
        /// <param name="secureLink">String representing the secure link for which the message is to be downloaded.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the decrypted message
        /// </returns>
        public String GetMessage(String secureLink)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetMessage(secureLink);
        }

        /// <summary>
        /// Returns packages in the current user's organization based on provided search criteria. The search defaults to returning all packages up to the current date and time, if a specific value is not passed for each search criteria. A maximum of 100 records will be returned per method call. The calling user must be a SendSafely Enterprise Administrator.
        /// </summary>
        /// <param name="fromDate">Date and time to search for packages with a package timestamp that is greater than or equal to the specified value. </param>
        /// <param name="toDate">Date and time to search for packages with a package timestamp that is less than or equal to the specified value. </param>
        /// <param name="sender">Email address to search for packages with a matching package sender email address. A valid email address must be provided.</param>
        /// <param name="status">PackageStatus enum value to search for packages with a matching package status.</param>
        /// <param name="recipient">Email address to search for packages with a matching recipient email address. A valid email address must be provided.</param>
        /// <param name="fileName">Name of a file to search for packages with a matching file name.</param>
        /// <returns>A PackageSearchResults object</returns>
        public PackageSearchResults GetOrganizationPackages(DateTime? fromDate, DateTime? toDate, String sender, PackageStatus? status, String recipient, String fileName)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetOrganizationPackages(fromDate, toDate, sender, status, recipient, fileName);
        }

        /// <summary>
        ///  Fetch the latest package meta data about a specific package given the unique package id.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the get package information operation.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A PackageInfo object containing package information.
        /// </returns>
        public PackageInformation GetPackageInformation(String packageId)
        {
            EnforceInitialized();

            if (packageId == null)
            {
                throw new InvalidPackageException("PackageID is null");
            }

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetPackageInformation(packageId);
        }

        /// <summary>
        /// Fetch the latest package meta data about a specific package given the secure link of type Uri.
        /// </summary>
        /// <param name="link">Uri object representing the secure link for which package information is to be fetched.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid link ID is used.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A PackageInformation object containing information about the package.
        /// </returns>
        public PackageInformation GetPackageInformationFromLink(Uri link)
        {
            EnforceInitialized();

            if (link == null)
            {
                throw new InvalidPackageException("The supplied link is null");
            }

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetPackageInformationFromLink(link);
        }

        [Obsolete("getPackageInformationFromLink is deprecated, please use GetPackageInformationFromLink instead", false)]
        public PackageInformation getPackageInformationFromLink(Uri link)
        {
            EnforceInitialized();

            if (link == null)
            {
                throw new InvalidPackageException("The supplied link is null");
            }

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetPackageInformationFromLink(link);
        }

        /// <summary>
        /// Fetch the latest package meta data about a specific package given a secure link of type String.
        /// </summary>
        /// <param name="link">String representing the secure link for which package information is to be fetched.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid link is used.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A PackageInformation object containing information about the package.
        /// </returns>
        public PackageInformation GetPackageInformationFromLink(String link)
        {
            EnforceInitialized();

            if (link == null)
            {
                throw new InvalidPackageException("The supplied link is null");
            }

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetPackageInformationFromLink(link);
        }

        /// <summary>
        /// Returns a secure link for accessing a package. This method is intended for generating a shareable link for a Workspace package, however non-Workspace packages are also supported. Packages with a temporary or deleted {@link PackageState} are not supported by this method.    
        /// </summary>
        /// <param name="packageId">The unique package id of the package to generate the secure link.</param>
        /// <param name="keyCode">The keycode belonging to the package.</param>
        /// <returns>The secure link to access the package.</returns>
        public String GetPackageLink(String packageId, String keyCode)
        {
            EnforceInitialized();

            PackageInformation packageInfo = GetPackageInformation(packageId);
            if (packageInfo.State.Equals(PackageState.PACKAGE_STATE_TEMP) || packageInfo.State.Equals(PackageState.PACKAGE_STATE_DELETED_COMPLETE) || packageInfo.State.Equals(PackageState.PACKAGE_STATE_DELETED_INCOMPLETE) || packageInfo.State.Equals(PackageState.PACKAGE_STATE_DELETED_PARTIALLY_COMPLETE))
            {
                throw new InvalidPackageException("Package link could not be generated due to unsupported package state");
            }
            String workspaceLink = this.connection.ApiHost + "/receive/?packageCode=" + packageInfo.PackageCode + "#keycode=" + keyCode;

            return workspaceLink;
        }

        /// <summary>
        /// Retrieves a list of all active packages received for the given API User.
        /// </summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="LimitExceededException">Thrown when the limits for the user has been exceeded.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A List containing package metadata for all received packages.
        /// </returns>
        public List<PackageInformation> GetReceivedPackages()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetReceivedPackages();
        }

        /// <summary>
        /// Retrieves a recipient from a given package.
        /// </summary>
        /// <param name="packageId"> The package id to retrieve the recipient from.</param>
        /// <param name="recipientId"> The recipient id to be retrieved from the package.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        /// <returns>
        /// Returns the recipient.
        /// </returns>
        public Recipient GetRecipient(String packageId, String recipientId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetRecipient(packageId, recipientId);
        }

        /// <summary>
        /// Retrieves a list of packages where the passed in email address is a package recipient.
        /// </summary>
        /// <param name="recipientEmail"> The email address of the recipient.</param>
        /// <returns>
        /// A PackageInfo object containing information about each package retrieved, including confirmed downloads for the recipient.
        /// </returns>
        public List<RecipientHistory> GetRecipientHistory(String recipientEmail)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetRecipientHistory(recipientEmail);
        }

        /// <summary>
        /// Gets information about the current logged in user.
        /// </summary>
        /// <returns>A User object.</returns>
        public User GetUserInformation()
        {
            EnforceInitialized();

            StartupUtility su = new StartupUtility(connection);
            return su.GetUserInformation();
        }

        /// <summary>
        /// Moves a directory to the specified destination directory in a Workspace package.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the move directory operation. </param>
        /// <param name="sourceDirectoryId">The unique directory id of the directory to move.</param>
        /// <param name="destinationDirectoryId">The unique directory id of the destination directory.</param>
        public void MoveDirectory(String packageId, String sourceDirectoryId, String destinationDirectoryId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.MoveDirectory(packageId, sourceDirectoryId, destinationDirectoryId);
        }

        /// <summary>
        /// Moves a file to the specified destination directory in a Workspace package.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the move file operation.</param>
        /// <param name="fileId">The unique file id of the file to move.</param>
        /// <param name="destinationDirectoryId">The unique directory id of the destination directory.</param>
        public void MoveFile(String packageId, String fileId, String destinationDirectoryId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.MoveFile(packageId, fileId, destinationDirectoryId);
        }

        /// <summary>
        /// This method is intended for use by the SendSafely Outlook Plugin. Before this can be called a valid oauth token must have been obtained.
        /// </summary>
        /// <param name="oauthToken">The Google oauth token used to look up the user.</param>
        /// <param name="keyDescription">A description describing the generated API key.</param>
        /// <exception cref="RegistrationNotAllowedException">Thrown when the user is not allowed to register.</exception>
        /// <exception cref="DuplicateUserException">Thrown when the user already has a valid username/password account.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the oauth token is incorrect.</exception>
        public APICredential OAuthGenerateAPIKey(String oauthToken, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.OAuthGenerateAPIKey(oauthToken, keyDescription);
        }

        /// <summary>
        /// Parses out SendSafely links from a String of text.
        /// </summary>
        /// <param name="text">The text containing links to parse.</param>
        /// <returns>
        /// A List<String> of SendSafely URLs
        /// </returns>
        public List<String> ParseLinksFromText(String text)
        {
            ParseLinksUtility handler = new ParseLinksUtility();
            return handler.Parse(text);
        }

        /// <summary>
        /// Remove a Contact Group from a package.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the remove the Contact Group operation.</param>
        /// <param name="groupId"> The unique id of the Contact Group to remove from the package.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void RemoveContactGroupFromPackage(String packageId, String groupId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.RemoveContactGroupFromPackage(packageId, groupId);
        }

        /// <summary>
        /// Deletes a recipient email address from the current user's Dropzone.
        /// </summary>
        /// <param name="email"> The recipient email address to delete from the Dropzone.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message as we expect the server to return a success.</exception>
        public void RemoveDropzoneRecipient(String email)
        {
            throw new NotImplementedException("This method is not currently supported");
        }

        /// <summary>
        /// Removes a recipient from a given package.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the remove recipient operation.</param>
        /// <param name="recipientId"> The unique recipient id of the recipient to remove from the package.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        public void RemoveRecipient(String packageId, String recipientId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.RemoveRecipient(packageId, recipientId);
        }

        /// <summary>
        /// Remove user email address from the specified Contact Group.
        /// </summary>
        /// <param name="groupId"> The unique id of the Contact Group for the remove user operation.</param>
        /// <param name="userId">The unique id of the user whose email address is to be removed from the Contact Group.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void RemoveUserFromContactGroup(String groupId, String userId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.RemoveUserFromContactGroup(groupId, userId);
        }

        /// <summary>
        /// Renames a directory to the specified directory name in a Workspace package.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the rename directory operation.</param>
        /// <param name="directoryId">The unique directory id of the directory to rename.</param>
        /// <param name="directoryName">The new name of the directory.</param>
        public void RenameDirectory(String packageId, String directoryId, String directoryName)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.RenameDirectory(packageId, directoryId, directoryName);
        }

        /// <summary>
        /// Revokes a public key from the server. Only call this if the private key has been deleted and should not be used anymore.
        /// </summary>
        /// <param name="publicKeyId">The public key id to revoke.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="RevokingKeyFailedException">Thrown if the revoke failed.</exception>
        public void RevokePublicKey(String publicKeyId)
        {
            EnforceInitialized();

            PublicKeyUtility utility = new PublicKeyUtility(this.connection);
            utility.RevokePublicKey(publicKeyId);
        }

        /// <summary>
        /// Updates the package descriptor. For a Workspaces package, this method can be used to change the name of the Workspace.
        /// </summary>
        /// <param name="packageId">The unique package id of the package for the descriptor update operation.</param>
        /// <param name="packageDescriptor">The string value to update the package descriptor to. </param>
        public void UpdatePackageDescriptor(String packageId, String packageDescriptor)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.UpdatePackageDescriptor(packageId, packageDescriptor);
        }

        /// <summary>
        /// Update the package life. Setting the life to 0 means the package will not expire.
        /// </summary>
        /// <param name="packageId"> The unique package id of the package for the update package life operation.</param>
        /// <param name="life"> The new package life. Setting this parameter to 0 means the package will not expire.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message or if the action is denied.</exception>
        /// <returns>
        /// A bool indicating if the call was successfull or not.
        /// </returns>
        public bool UpdatePackageLife(String packageId, int life)
        {
            EnforceInitialized();

            if (packageId == null)
            {
                throw new InvalidPackageException("PackageID is null");
            }

            PackageUtility pu = new PackageUtility(connection);
            return pu.UpdatePackageLife(packageId, life);
        }

        /// <summary>
        /// Used to update the role of a Workspace package recipient (i.e. Workspace collaborator).
        /// </summary>
        /// <param name="packageId">The unique package id of the Workspace package for the update role operation.</param>
        /// <param name="recipientId">The unique recipient id for the Workspace collaborator whose role is to be updated. </param>
        /// <param name="role">String representing the role for the update role operation. Supported values are VIEWER, CONTRIBUTOR, MANAGER, and OWNER.</param>
        public void UpdateRecipientRole(String packageId, String recipientId, String role)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.UpdateRecipientRole(packageId, recipientId, role);
        }

        /// <summary>
        /// Verifies a user's API key and secret. This method is typically called when a new user uses the API for the first time. 
        /// </summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        /// <returns>
        /// String containing the email belonging to the authenticated user.
        /// </returns>
        public String VerifyCredentials()
        {
            EnforceInitialized();

            StartupUtility su = new StartupUtility(connection);
            String email = su.VerifyCredentials();
            return email;
        }

        /// <summary>
        /// Verifies the current version of the SendSafely API against the server. Returns an enum describing if the API needs to be updated or not.
        /// </summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message.</exception>
        /// <returns>
        /// Returns OK, UPGRADE_AVAILABLE or OUTDATED_VERSION depending on the response from the Server.
        /// </returns>
        public Objects.Version VerifyVersion()
        {
            EnforceInitialized();

            StartupUtility su = new StartupUtility(connection);
            Objects.Version versionResponse = su.VerifyVersion();
            return versionResponse;
        }

#region Outlook Plugin methods
        /// <summary>
        /// This method is intended for use by the SendSafely Outlook Plugin. Sets the outlook version of the outlook client.
        /// </summary>
        /// <param name="version">Version value of outlook.</param>
        public void SetOutlookVersion(String version)
        {
            this.connection.OutlookVersion = version;
        }

        /// <summary>
        /// This method is intended for use by the SendSafely Outlook Plugin. Starts the registration process. If a valid email is provided, a validation code will be sent to the SendSafely servers.
        /// </summary>
        /// <param name="email">The email to register</param>
        /// <exception cref="InvalidEmailException">Thrown when an incorrect email is used.</exception>
        /// <exception cref="RegistrationNotAllowedException">Thrown when the user is not allowed to register.</exception>
        /// <exception cref="ActionFailedException">Thrown when the request failed for any other reason.</exception>
        public void StartPINRegistration(String email)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            util.StartPINRegistration(email);
        }

        /// <summary>
        /// This method is intended for use by the SendSafely Outlook Plugin. Starts the registration process. If a valid email is provided, a validation code will be sent to the SendSafely servers.
        /// </summary>
        /// <param name="email">The email to register.</param>
        /// <exception cref="InvalidEmailException">Thrown when an incorrect email is used.</exception>
        /// <exception cref="ActionFailedException">Thrown when the request failed for any other reason.</exception>
        public void StartRegistration(String email)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            util.StartRegistration(email);
        }

        /// <summary>
        /// This method is intended for use by the SendSafely Outlook Plugin. Before this can be called a pin code must have been obtained.
        /// </summary>
        /// <param name="email">The email to the user to finish registration for.</param>
        /// <param name="pincode">The pincode sent to the users email.</param>
        /// <param name="password">The desired password.</param>
        /// <param name="secretQuestion">The secret question which is to be associated with the account.</param>
        /// <param name="answer">The answer answering the secret question.</param>
        /// <param name="firstName">The First name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="keyDescription">A description describing the generated API key.</param>
        /// <exception cref="InsufficientPasswordComplexityException">Thrown when the password is to simple.</exception>
        /// <exception cref="TokenExpiredException">Thrown when the passed in token has expired.</exception>
        /// <exception cref="PINRefreshException">Thrown when a new Email PIN has been sent to the user.</exception>
        /// <exception cref="InvalidTokenException">Thrown when passed in token is incorrect.</exception>
        public APICredential FinishPINRegistration(String email, String pincode, String password, String secretQuestion, String answer, String firstName, String lastName, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.FinishPINRegistration(email, pincode, password, secretQuestion, answer, firstName, lastName, keyDescription);
        }

        /// <summary>
        /// This method is intended for use by the SendSafely Outlook Plugin. Finishes the registration process. Before this can be called a valid token must have been obtained.
        /// </summary>
        /// <param name="validationLink">The validation link which was sent to the specified email address.</param>
        /// <param name="password">The desired password.</param>
        /// <param name="secretQuestion">The secret question which is to be associated with the account.</param>
        /// <param name="answer">The answer answering the secret question.</param>
        /// <param name="firstName">The First name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="keyDescription">A description describing the generated API key.</param>
        /// <exception cref="InsufficientPasswordComplexityException">Thrown when the password is to simple.</exception>
        /// <exception cref="TokenExpiredException">Thrown when the passed in token has expired.</exception>
        /// <exception cref="InvalidTokenException">Thrown when passed in token is incorrect.</exception>
        public APICredential FinishRegistration(String validationLink, String password, String secretQuestion, String answer, String firstName, String lastName, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.FinishRegistration(validationLink, password, secretQuestion, answer, firstName, lastName, keyDescription);
        }
        #endregion

        private void EnforceInitialized()
        {
            if (connection == null)
            {
                throw new APINotInitializedException();
            }
        }

    }
}
