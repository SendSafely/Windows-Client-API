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
        /// Retrieves the information for the current package.</summary>
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
            EnforceInitialized();

            PackageUtility pu = new PackageUtility(connection);
            return pu.DownloadFile(packageId, fileId, keyCode, progress, downloadAPI);
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
        public Recipient AddRecipient(String packageId, string email)
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

        private void EnforceInitialized()
        {
            if (connection == null)
            {
                throw new APINotInitializedException();
            }
        }

    }
}
