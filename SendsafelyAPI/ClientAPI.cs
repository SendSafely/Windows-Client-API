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
        /// Initializes the API. This function must be called before the API can be used.</summary>
        /// <param name="host"> The host which the API will connect to.</param>
        /// <param name="apiKey"> The API Key which will be used to connect to SendSafely.</param>
        /// <param name="apiSecret"> The private key used to verify the request.</param>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        public void InitialSetup(string host, string apiKey, string apiSecret)
        {
            InitialSetup(host, apiKey, apiSecret, (WebProxy)null);
        }

        /// <summary>
        /// Initializes the API. This function must be called before the API can be used.</summary>
        /// <param name="host"> The host which the API will connect to.</param>
        /// <param name="apiKey"> The API Key which will be used to connect to SendSafely.</param>
        /// <param name="apiSecret"> The private key used to verify the request.</param>
        /// <param name="proxy"> A web proxy object which already contains proxy information.</param>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        public void InitialSetup(string host, string apiKey, string apiSecret, WebProxy proxy)
        {
            StartupUtility su = new StartupUtility(host, apiSecret, apiKey, proxy);
            this.connection = su.GetConnectionObject();
        }

        public void InitialSetup(string host, WebProxy proxy)
        {
            this.connection = new Connection(host, proxy);
        }

        public void InitialSetup(string host)
        {
            InitialSetup(host, (WebProxy)null);
        }

        public User GetUserInformation()
        {
            EnforceInitialized();

            StartupUtility su = new StartupUtility(connection);
            return su.GetUserInformation();
        }

        /// <summary>
        /// Starts the registration process. If a valid email is provided, a validation code will be sent to the SendSafely servers.
        /// </summary>
        /// <param name="email">The email to register</param>
        /// <exception cref="InvalidEmailException">Thrown when an incorrect email is used.</exception>
        /// <exception cref="ActionFailedException">Thrown when the request failed for any other reason.</exception>
        public void StartRegistration(String email)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            util.StartRegistration(email);
        }

        /// <summary>
        /// Starts the registration process. If a valid email is provided, a validation code will be sent to the SendSafely servers.
        /// </summary>
        /// <param name="email">The email to register</param>
        /// <exception cref="InvalidEmailException">Thrown when an incorrect email is used.</exception>
        /// <exception cref="RegistrationNotAllowedException">Thrown when the user is not allowed to register</exception>
        /// <exception cref="ActionFailedException">Thrown when the request failed for any other reason.</exception>
        public void StartPINRegistration(String email)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            util.StartPINRegistration(email);
        }

        /// <summary>
        /// Finishes the registration process. Before this can be called a valid token must have been obtained.
        /// </summary>
        /// <param name="validationLink">The validation link which was sent to the specified email address</param>
        /// <param name="password">The desired password</param>
        /// <param name="secretQuestion">The secret question which is to be associated with the account</param>
        /// <param name="answer">The answer answering the secret question</param>
        /// <param name="firstName">The First name of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="keyDescription">A description describing the generated API key</param>
        /// <exception cref="InsufficientPasswordComplexityException">Thrown when the password is to simple</exception>
        /// <exception cref="TokenExpiredException">Thrown when the passed in token has expired</exception>
        /// <exception cref="InvalidTokenException">Thrown when passed in token is incorrect</exception>
        public APICredential FinishRegistration(String validationLink, String password, String secretQuestion, String answer, String firstName, String lastName, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.FinishRegistration(validationLink, password, secretQuestion, answer, firstName, lastName, keyDescription);
        }

        /// <summary>
        /// Finishes the registration process. Before this can be called a valid oauth token must have been obtained.
        /// </summary>
        /// <param name="oauthToken">The Google oauth token used to look up the user</param>
        /// <param name="keyDescription">A description describing the generated API key</param>
        /// <exception cref="RegistrationNotAllowedException">Thrown when the user is not allowed to register</exception>
        /// <exception cref="DuplicateUserException">Thrown when the user already has a valid username/password account</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the oauth token is incorrect</exception>
        public APICredential OAuthGenerateAPIKey(String oauthToken, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.OAuthGenerateAPIKey(oauthToken, keyDescription);
        }

        /// <summary>
        /// Finishes the registration process. Before this can be called a pin code must have been obtained.
        /// </summary>
        /// <param name="email">The email to the user to finish registration for</param>
        /// <param name="pincode">The pincode sent to the users email</param>
        /// <param name="password">The desired password</param>
        /// <param name="secretQuestion">The secret question which is to be associated with the account</param>
        /// <param name="answer">The answer answering the secret question</param>
        /// <param name="firstName">The First name of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="keyDescription">A description describing the generated API key</param>
        /// <exception cref="InsufficientPasswordComplexityException">Thrown when the password is to simple</exception>
        /// <exception cref="TokenExpiredException">Thrown when the passed in token has expired</exception>
        /// <exception cref="PINRefreshException">Thrown when a new Email PIN has been sent to the user</exception>
        /// <exception cref="InvalidTokenException">Thrown when passed in token is incorrect</exception>
        public APICredential FinishPINRegistration(String email, String pincode, String password, String secretQuestion, String answer, String firstName, String lastName, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.FinishPINRegistration(email, pincode, password, secretQuestion, answer, firstName, lastName, keyDescription);
        }

        /// <summary>
        /// Generate a new API Key given a username and password.
        /// </summary>
        /// <param name="username">The email address of the given user.</param>
        /// <param name="password">The password belonging to the user</param>
        /// <param name="keyDescription">A description describing the generated API key</param>
        /// <exception cref="TwoFactorAuthException">Thrown when two factor authentication is required. The exception contains a ValidationToken that must be used in the subsequent request</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the username/password combination is wrong.</exception>
        /// <exception cref="ActionFailedException">Thrown if any other error occurs</exception>
        public APICredential GenerateAPIKey(String username, String password, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.GenerateAPIKey(username, password, keyDescription);
        }

        /// <summary>
        /// Generate a new API Key given a validation token and a SMS Code.
        /// </summary>
        /// <param name="validationLink">The validation link associated with the user.</param>
        /// <param name="smsCode">The smsCode sent to the users phone</param>
        /// <param name="keyDescription">A description describing the generated API key</param>
        /// <exception cref="InvalidCredentialsException">Thrown if the SMS Code is incorrect</exception>
        /// <exception cref="PINRefreshException">Thrown if there's been to many failed attempts and a new SMS code is needed</exception>
        /// <exception cref="ActionFailedException">Thrown if any other error occured</exception>
        public APICredential GenerateKey2FA(String validationLink, String smsCode, String keyDescription)
        {
            RegistrationUtility util = new RegistrationUtility(connection);
            return util.GenerateAPIKey2FA(validationLink, smsCode, keyDescription);
        }

        public void SetOutlookVersion(String version)
        {
            this.connection.OutlookVersion = version;
        }

        /// <summary>
        /// Verifies the version using the API Key.</summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
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

        /// <summary>
        /// Verifies the API and secret key against the server. Returns the user email which is associated with the key.</summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the users email.
        /// </returns>
        public String VerifyCredentials()
        {
            EnforceInitialized();

            StartupUtility su = new StartupUtility(connection);
            String email = su.VerifyCredentials();
            return email;
        }

        /// <summary>
        /// Generates a new RSA Key pair used to encrypt keycodes. The private key as well as an identifier associating the public Key is returned to the user. 
        /// The public key is uploaded and stored on the SendSafely servers.</summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
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
        /// Revokes a public key from the server. Only call this if the private key has been deleted and should not be used anymore.</summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="RevokingKeyFailedException">Thrown if the revoke failed</exception>
        public void RevokePublicKey(String publicKeyId)
        {
            EnforceInitialized();

            PublicKeyUtility utility = new PublicKeyUtility(this.connection);
            utility.RevokePublicKey(publicKeyId);
        }

        /// <summary>
        /// Downloads and decrypts a keycode from the Server given a packageId and a private key. 
        /// </summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="GettingKeycodeFailedException">Will be thrown if the server returns an error message while downloading the keycode</exception>
        /// <returns>
        /// Returns the decrypted keycode
        /// </returns>
        public String GetKeycode(PrivateKey privateKey, String packageId)
        {
            EnforceInitialized();

            PublicKeyUtility utility = new PublicKeyUtility(this.connection);
            return utility.GetKeycode(packageId, privateKey);
        }

        /// <summary>
        /// Parses out SendSafely links from a String of text.
        /// </summary>
        /// <returns>
        /// Returns a list of SendSafely links found in the text String.
        /// </returns>
        public List<String> ParseLinksFromText(String text)
        {
            ParseLinksUtility handler = new ParseLinksUtility();
            return handler.Parse(text);
        }

        /// <summary>
        /// Retrieves enterprise related information such as the hostname specific to the organization as well as the SendSafely name of the organization.</summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
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
        /// Creates a new package and returns the package information of the newly created package.</summary>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="LimitExceededException">Thrown when the limits for the user has been exceeded.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
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
        /// Creates a new workspace (package) and returns the package information of the newly created package.
        /// </summary>
        /// <param name="isWorkspace"></param>
        /// <returns>Returns the package information object on the newly created package</returns>
        public PackageInformation CreatePackage(bool isWorkspace)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.CreatePackage(isWorkspace);
        }

        /// <summary>
        /// Creates a new package on behalf of the specified user and returns the package information of the newly created package. API user
        /// must be a SendSafely administrator.
        /// </summary>
        /// <param name="email"> The email address of the user to create the package as (only for enterprise)</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="LimitExceededException">Thrown when the limits for the user has been exceeded.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
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
        /// Creates a directory and returns the directory information
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="parentDirectoryId"></param>
        /// <param name="directoryName"></param>
        /// <returns>Returns the package information object on the newly created package</returns>
        public Directory CreateDirectory(String packageId, String parentDirectoryId, String directoryName)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            String directoryId = pu.CreateDirectory(packageId, parentDirectoryId, directoryName);
            return this.GetDirectory(packageId, directoryId);
        }

        /// <summary>
        /// Deletes a target directory within a package
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="directoryId"></param>
        public void DeleteDirectory(String packageId, String directoryId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.DeleteDirectory(packageId, directoryId);
        }


        /// <summary>
        /// Retrieves a list of all active packages for the given API Key.</summary>
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
        /// Retrieves a list of all active recieved packages for the given API User.</summary>
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
        /// Retrieves a list of all active packages for the given API Key.</summary>
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

        public PackageSearchResults GetOrganizationPackages(DateTime? fromDate, DateTime? toDate, String sender, PackageStatus? status, String recipient, String fileName)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetOrganizationPackages(fromDate, toDate, sender, status, recipient,fileName);
        }

        /// <summary>
        /// Retrieves all packages with given Recipient.</summary>
        /// <param name="recipientEmail"> The recipient Email for which the packages information should be fetched.</param>
        /// <returns>
        /// A PackageInfo object containing information about the package.
        /// </returns>
        public List<RecipientHistory> GetRecipientHistory(String recipientEmail)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetRecipientHistory(recipientEmail);
        }

        /// <summary>
        /// Gets the directory from a given directory id
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="directoryId"></param>
        /// <returns>Returns the directory object of the newly created directory</returns>
        public Directory GetDirectory(String packageId, String directoryId)
        {
            EnforceInitialized();
            PackageUtility pu = new PackageUtility(connection);
            return pu.GetDirectory(packageId, directoryId);

        }

        /// <summary>
        /// Moves a directory to a new directory
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="sourceDirectoryId"></param>
        /// <param name="destinationDirectoryId"></param>
        public void MoveDirectory(String packageId, String sourceDirectoryId, String destinationDirectoryId) {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.MoveDirectory(packageId, sourceDirectoryId, destinationDirectoryId);
        }

        /// <summary>
        /// Renames a target directory
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="directoryId"></param>
        /// <param name="directoryName"></param>
        public void RenameDirectory(String packageId, String directoryId, String directoryName)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.RenameDirectory(packageId, directoryId, directoryName);
        }

        /// <summary>
        /// Move a given file to a new directory
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="fileId"></param>
        /// <param name="destinationDirectoryId"></param>
        public void MoveFile(String packageId, String fileId, String destinationDirectoryId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.MoveFile(packageId, fileId, destinationDirectoryId);
        }

        /// <summary>
        /// Retrieves the information for the current package.</summary>
        /// <param name="packageId"> The package ID for which information should be fetched.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// A PackageInfo object containing information about the package.
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
    /// Retrieves the information for the current package given a SendSafely link.</summary>
    /// <param name="link">The link identiying the package.</param>
    /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
    /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
    /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid link is used.</exception>
    /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
    /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
    /// <returns>
    /// A PackageInfo object containing information about the package.
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
    /// Retrieves the information for the current package given a SendSafely link.</summary>
    /// <param name="packageId">The link identiying the package.</param>
    /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
    /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
    /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid link ID is used.</exception>
    /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
    /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
    /// <returns>
    /// A PackageInfo object containing information about the package.
    /// </returns>
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
        /// Retrieves the information for the current package.</summary>
        /// <param name="packageId"> The package ID for which information should be fetched.</param>
        /// <param name="life"> The new life.</param>
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
        /// Uploads a file to the given package. Before this function can be called, the package must have been created with 
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.</summary>
        /// <param name="packageId"> The packageId which is identifying the package to where a file should be added.</param>
        /// <param name="path">The path to the file.</param>
        /// <param name="progress">A progress object to where progress will be called back to.</param>
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
        /// Uploads a file to the given package. Before this function can be called, the package must have been created with 
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.</summary>
        /// <param name="packageId"> The packageId which is identifying the package to where a file should be added.</param>
        /// <param name="keyCode">The key which will be used to encrypt the file with.</param>
        /// <param name="path">The path to the file.</param>
        /// <param name="progress">A progress object to where progress will be called back to.</param>
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

        public File EncryptAndUploadFile(String packageId, String keyCode, String path, ISendSafelyProgress progress, String uploadType)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);

            File file = pu.EncryptAndUploadFile(packageId, keyCode, path, progress, uploadType);
            return file;
        }

        /// <summary>
        /// Encrypts and uploads a file in a directory
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="directoryId"></param>
        /// <param name="keyCode"></param>
        /// <param name="path"></param>
        /// <param name="progress"></param>
        /// <returns>Returns the newly uploaded file object</returns>
        public File EncryptAndUploadFileInDirectory(String packageId, String directoryId, String keyCode, String path, ISendSafelyProgress progress)
        {
            EnforceInitialized();

            return EncryptAndUploadFileInDirectory(packageId, directoryId, keyCode, path, progress, "CSHARP");
        }


        /// <summary>
        /// Encrypts and uploads a file in a directory
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="directoryId"></param>
        /// <param name="keyCode"></param>
        /// <param name="path"></param>
        /// <param name="progress"></param>
        /// <param name="uploadType"></param>
        /// <returns>Returns the newly uploaded file object</returns>
        public File EncryptAndUploadFileInDirectory(String packageId, String directoryId, String keyCode, String path, ISendSafelyProgress progress, String uploadType)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);

            File file = pu.EncryptAndUploadFile(packageId, directoryId, keyCode, path, progress, uploadType);
            return file;
        }



        /// <summary>
        /// Adds a message to the given package. Before this function can be called, the package must have been created with 
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.</summary>
        /// <param name="packageId"> The packageId which is identifying the package to where a file should be added.</param>
        /// <param name="message">The message to encrypt and upload.</param>
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
        /// Adds a message to the given package. Before this function can be called, the package must have been created with 
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.</summary>
        /// <param name="packageId"> The packageId which is identifying the package to where a file should be added.</param>
        /// <param name="keyCode">The key which will be used to encrypt the file with.</param>
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

        public void EncryptAndUploadMessage(String packageId, String keyCode, String message, String uploadType)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);

            pu.EncryptAndUploadMessage(packageId, keyCode, message, uploadType);
        }

        /// <summary>
        /// Gets file information on a file within a given directory
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="directoryId"></param>
        /// <param name="fileId"></param>
        /// <returns>Returns information on the file </returns>
        public FileInformation GetFileInformation(String packageId, String directoryId, String fileId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);

            return pu.GetFileInformation(packageId, directoryId, fileId);
        }

        /// <summary>
        /// Downloads and decrypts a message for the given secure link
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.</summary>
        /// <param name="secureLink">The link from where to download the message</param>
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
        /// Downloads and decrypts a file given a package and a file ID
        /// </summary>
        /// <param name="packageId">The Package Identifier to grab the file from</param>
        /// <param name="fileId">The file to download</param>
        /// <param name="keyCode">The key which will be used to decrypt the file with.</param>
        /// <param name="progress">A progress object to where progress will be called back to.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the decrypted file
        /// </returns>
        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, ISendSafelyProgress progress)
	    {
            return DownloadFile(packageId, fileId, keyCode, progress, "CSHARP");
	    }

        /// <summary>
        /// Downloads and decrypts a file given a package and a file ID
        /// </summary>
        /// <param name="packageId">The Package Identifier to grab the file from</param>
        /// <param name="fileId">The file to download</param>
        /// <param name="keyCode">The key which will be used to decrypt the file with.</param>
        /// <param name="progress">A progress object to where progress will be called back to.</param>
        /// <param name="downloadAPI">A String identifying the API which is downloading the file. Defaults to "CSHARP".</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the decrypted file
        /// </returns>
        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, ISendSafelyProgress progress, String downloadAPI)
        {
            return DownloadFile(packageId, fileId, keyCode, progress, downloadAPI, null);
        }

        /// <summary>
        /// Downloads and decrypts a file given a package and a file ID
        /// </summary>
        /// <param name="packageId">The Package Identifier to grab the file from</param>
        /// <param name="fileId">The file to download</param>
        /// <param name="keyCode">The key which will be used to decrypt the file with.</param>
        /// <param name="progress">A progress object to where progress will be called back to.</param>
        /// <param name="downloadAPI">A String identifying the API which is downloading the file. Defaults to "CSHARP".</param>
        /// <param name="password">Password for protecting a package when recipients are not specified.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the decrypted file
        /// </returns>
        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, ISendSafelyProgress progress, String downloadAPI, String password)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.DownloadFile(packageId, null, fileId, keyCode, progress, downloadAPI, password);
        }

        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, String password)
        {
            return DownloadFile(packageId, fileId, keyCode, null, "CSHARP", password);
        }

        public FileInfo DownloadFile(String packageId, String fileId, String keyCode, String password, ISendSafelyProgress progress)
        {
            return DownloadFile(packageId, fileId, keyCode, progress, "CSHARP", password);
        }


        /// <summary>
        /// Downloads a file from a directory
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="directoryId"></param>
        /// <param name="fileId"></param>
        /// <param name="keyCode"></param>
        /// <param name="progress"></param>
        /// <returns>System file info object</returns>
        public FileInfo DownloadFileFromDirectory(String packageId, String directoryId, String fileId, String keyCode, ISendSafelyProgress progress)
        {
            return DownloadFileFromDirectory(packageId, directoryId, fileId, keyCode, progress, "CSHARP");
        }


        /// <summary>
        /// downloads a file from a directory
        /// 
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="directoryId"></param>
        /// <param name="fileId"></param>
        /// <param name="keyCode"></param>
        /// <param name="progress"></param>
        /// <returns>System File Info Object</returns>
        public FileInfo DownloadFileFromDirectory(String packageId, String directoryId, String fileId, String keyCode, ISendSafelyProgress progress, String api)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.DownloadFile(packageId, directoryId, fileId, keyCode, progress, api, null);
        }

        /// <summary>
        /// Deletes a file from a workspace directory
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="directoryId"></param>
        /// <param name="fileId"></param>
        public void DeleteFile(String packageId, String directoryId, String fileId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.DeleteFile(packageId, directoryId, fileId);
        }
        /// <summary>
        /// Adds a recipient to a given package. Before this function can be called, the package must have been created with 
        /// <seealso cref="CreatePackage()">createPackage()</seealso>.</summary>
        /// <param name="packageId"> The packageId referencing the package to where a new recipient should be added.</param>
        /// <param name="email"> The recipient email to be added.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="InvalidEmailException">Thrown when the given email is incorrect.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
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
        /// Updates a recipient by giving it a phone number. A package must have been created and the recipient 
        /// must have been added before this function can be called. 
        /// See <seealso cref="CreatePackage()">createPackage()</seealso></summary>
        /// <param name="packageId"> The packageId referencing the package to where the recipient belongs.</param>
        /// <param name="recipientId"> The recipientId to which, a phone number should be added.</param>
        /// <param name="phoneNumber"> The phonenumber to add.</param>
        /// <param name="countryCode"> The country code to where the phone number belongs.</param>
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
        /// Adds a dropzone recipient
        /// <param name="email"> The email address for the dropzone recipient.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void AddDropzoneRecipient(String email)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.AddDropzoneRecipient(email);
        }

        /// <summary>
        /// gets dropzone recipients
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message as we expect the server to return a success.</exception>
        /// <returns>
        /// Returns the list of dropzone recipient email addresses
        /// </returns>
        public List<String> GetDropzoneRecipients()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetDropzoneRecipients();
        }

        /// <summary>
        /// Adds a dropzone recipient
        /// <param name="email"> The email address for the dropzone recipient to remove.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message as we expect the server to return a success.</exception>
        public void RemoveDropzoneRecipient(String email)
        {
            throw new NotImplementedException("This method is not currently supported");
        }


        /// <summary>
        /// Creates a contact group
        /// <param name="email"> The email address for the dropzone recipient.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the ID of the contact group
        /// </returns>
        public String CreateContactGroup(String groupName)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.CreateContactGroup(groupName);

        }

        /// <summary>
        /// Deletes a contact group
        /// <param name="groupId"> The group id of the contact group to be deleted.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void DeleteContactGroup(String groupId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.DeleteContactGroup(groupId);
        }

        /// <summary>
        /// Adds a user to a contact group
        /// <param name="groupId"> The group id of the contact group to be deleted.</param>
        /// <param name="userEmail"> The email address of the user to be added to the contact group. </param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the user id of the user added to the contact group
        /// </returns>
        public String AddUserToContactGroup(String groupId, String userEmail)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.AddGroupUser(groupId, userEmail);
        }

        /// <summary>
        /// Deletes a contact group user
        /// <param name="groupId"> The group id of the contact group to be deleted.</param>
        /// <param name="userEmail"> The email address of the user to be added to the contact group. </param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void RemoveUserFromContactGroup(String groupId, String userId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.RemoveUserFromContactGroup(groupId, userId);
        }

        /// <summary>
        /// Gets a list of the contact groups
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the list of contact groups
        /// </returns>
        public List<ContactGroup> getContactGroups()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetContactGroups(false);
        }

        /// <summary>
        /// Gets a list of the contact groups
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the list of contact groups
        /// </returns>
        public List<ContactGroup> getEnterpriseContactGroups()
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetContactGroups(true);
        }

        /// <summary>
        /// adds a list of recipients to the package
        /// <param name="packageId"> The package id to add the contact group as a recipient </param>
        /// <param name="groupId"> The group id of the contact group to be deleted.</param>
        /// <exception cref="LimitExceededException">Will be thrown if the server returns a limit exceeded message</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the list of recipients
        /// </returns>
        public List<Recipient> AddRecipients(String packageId, List<String> emails)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.AddRecipients(packageId, emails);
        }

        /// <summary>
        /// adds a contact group as a recipient to the package
        /// <param name="packageId"> The package id to add the contact group as a recipient </param>
        /// <param name="groupId"> The group id of the contact group.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void AddContactGroupToPackage(String packageId, String groupId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.AddContactGroupToPackage(packageId, groupId);
        }

        /// <summary>
        /// removes the contact group from the package
        /// <param name="packageId"> The package id to remove the contact group as a recipient </param>
        /// <param name="groupId"> The group id of the contact group.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void RemoveContactGroupFromPackage(String packageId, String groupId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.RemoveContactGroupFromPackage(packageId, groupId);
        }

        /// <summary>
        /// Removes a recipient from a package
        /// <param name="packageId"> The package id to remove the recipient from  </param>
        /// <param name="recipientId"> The recipient id to be removed from the package.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        public void RemoveRecipient(String packageId, String recipientId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.RemoveRecipient(packageId, recipientId);
        }

        /// <summary>
        /// Retrieves a recipient from a package
        /// <param name="packageId"> The package id to retrieve the recipient from  </param>
        /// <param name="recipientId"> The recipient id to be retrieved from the package.</param>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the recipient
        /// </returns>
        public Recipient GetRecipient(String packageId, String recipientId)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetRecipient(packageId, recipientId);
        }

        public String GetWorkSpaceLink(String packageId, String keyCode)
        {
            EnforceInitialized();

            PackageInformation packageInfo = GetPackageInformation(packageId);

            if (!packageInfo.IsWorkspace)
                throw new InvalidPackageException("Supplied packageId is not for a Workspace");

            String workspaceLink = this.connection.ApiHost + "/receive/?packageCode=" + packageInfo.PackageCode + "#keycode=" + keyCode;

            return workspaceLink;
        }

        /// <summary>
        /// Finalizes the package so it can be delivered to the recipients. Before this function can be called, the package must have been created with 
        /// <seealso cref="CreatePackage()">createPackage()</seealso>. Additionally, the package must contain at least one file.</summary>
        /// <param name="packageId"> The packageID referencing the object which should be finalized.</param>
        /// <param name="keycode"> The keycode, which will be included in the link which should be delivered to the recipients.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="PackageFinalizationException">Thrown when the package couldn't be finalized. The message will contain detailed information</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the link needed to access the package.
        /// </returns>
        public String FinalizePackage(String packageId, String keycode)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.FinalizePackage(packageId, keycode);
        }

        /// <summary>
        /// Finalizes a package without a recipient
        /// <param name="packageId"> The packageID referencing the object which should be finalized.</param>
        /// <param name="keycode"> The keycode, which will be included in the link which should be delivered to the recipients.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="PackageFinalizationException">Thrown when the package couldn't be finalized. The message will contain detailed information</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the link needed to access the package.
        /// </returns>
        public String FinalizeUndisclosedPackage(String packageId, String keycode)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.FinalizeUndisclosedPackage(packageId, keycode);
        }

        /// <summary>
        /// Finalizes a package without a recipient but protects it with a password
        /// <param name="packageId"> The packageID referencing the object which should be finalized.</param>
        /// <param name="password"> The password to access the package.</param>
        /// <param name="keycode"> The keycode, which will be included in the link which should be delivered to the recipients.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="LimitExceededException">Thrown when the package limits has been exceeded.</exception>
        /// <exception cref="PackageFinalizationException">Thrown when the package couldn't be finalized. The message will contain detailed information</exception>
        /// <exception cref="MissingKeyCodeException">Thrown when the keycode is null, empty or to short.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message</exception>
        /// <returns>
        /// Returns the link needed to access the package.
        /// </returns>
        public String FinalizeUndisclosedPackage(String packageId, String password, String keycode)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.FinalizeUndisclosedPackage(packageId, password, keycode);
        }

        /// <summary>
        /// Deletes a temporary package. This action must be called before the package is finalized. Before this function can be called, the package must have been created with 
        /// <seealso cref="CreatePackage()">CreatePackage()</seealso>.</summary>
        /// <param name="packageId"> The packageID referencing the object which should be deleted.</param>
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
        /// Deletes a package. Before this function can be called, the package must have been created with 
        /// <seealso cref="CreatePackage()">CreatePackage()</seealso>.</summary>
        /// <param name="packageId"> The packageID referencing the object which should be deleted.</param>
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
        /// Updates a package name
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="packageDescriptor"></param>
        public void UpdatePackageDescriptor(String packageId, String packageDescriptor)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.UpdatePackageDescriptor(packageId, packageDescriptor);
        }

        /// <summary>
        /// updates a recipient role
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="recipientId"></param>
        /// <param name="role"></param>
        public void UpdateRecipientRole(String packageId, String recipientId, String role)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            pu.UpdateRecipientRole(packageId, recipientId, role);
        }

        /// <summary>
        /// Gets the activity log on a package
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="rowIndex"></param>
        /// <returns>List of Actiity Log Entries</returns>
        public List<ActivityLogEntry> GetActivityLog(String packageId, int rowIndex)
        {
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.GetActivityLog(packageId, rowIndex);
        }

        private void EnforceInitialized()
        {
            if (connection == null)
            {
                throw new APINotInitializedException();
            }
        }

    }
}
