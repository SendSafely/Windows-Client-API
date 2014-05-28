using System;
using System.Collections.Generic;
using System.Text;
using SendSafely.Objects;
using SendSafely.Utilities;
using SendSafely.Exceptions;
using System.Net;

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
        /// Retrieves the information for the current package.</summary>
        /// <param name="packageId"> The package ID for which information should be fetched.</param>
        /// <param name="life"> The new life.</param>
        /// <exception cref="APINotInitializedException">Thrown when the API has not been initialized.</exception>
        /// <exception cref="InvalidCredentialsException">Thrown when the API credentials are incorrect.</exception>
        /// <exception cref="InvalidPackageException">Thrown when a non-existent or invalid package ID is used.</exception>
        /// <exception cref="ServerUnavailableException">Thrown when the API failed to connect to the server.</exception>
        /// <exception cref="ActionFailedException">Will be thrown if the server returns an error message or if the action is denied.</exception>
        /// <returns>
        /// A bool indicating if the call was successful or not.
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

        private void EnforceInitialized()
        {
            if (connection == null)
            {
                throw new APINotInitializedException();
            }
        }

    }
}
