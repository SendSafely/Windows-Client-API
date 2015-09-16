using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SendSafely;

namespace SendSafelyConsoleApplication
{
    class Program
    {
        // This is a sample console application designed to show basic use of the SendSafely .NET API
        static void Main(string[] args)
        {
            /*
             * This example will read in the following command line arguments:
             *
             *        SendSafelyHost: The SendSafely hostname to connect to.  Enterprise users should connect to their designated 
             *                        Enterprise host (company-name.sendsafely.com)
             *
             *            UserApiKey: The API key for the user you want to connect to.  API Keys can be obtained from the Edit 
             *                        Profile screen when logged in to SendSafely
             *
             *         UserApiSecret: The API Secret associated with the API Key used above.  The API Secret is provided to  
             *                        you when you generate a new API Key.  
             *
             *          FileToUpload: Local path to the file you want to upload. Can be any file up to 2GB in size. 
             *
             * RecipientEmailAddress: The email address of the person you want to send the file to.
             *
             *    RecipientSMSNumber: Optional argument. The mobile number associated with the email recipient.  This is optional 
             *                        and only used if you want to enable SMS authenticaiton for the recipient. 
             */

            if (args == null || (args.Length != 5 && args.Length != 6))
            {
                // Invalid number of arguments.  Print the usage syntax to the screen and exit. 
                Console.WriteLine("Usage: " + System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + " SendSafelyHost UserApiKey UserApiSecret FileToUpload RecipientEmailAddress [RecipientSMSNumber]");
                Console.WriteLine("\nThis program will print out the secure URL to access the package after it has been submitted.");
                Console.ReadLine();
                return;
            }
            else
            {
                // Valid arguments provided.  Assign each argument to a local variable 
                String sendSafelyHost = args[0];
                String userApiKey = args[1];
                String userApiSecret = args[2];
                String fileToUpload = args[3];
                String recipientEmailAddress = args[4];
                String recipientSMSNumber = String.Empty;
                String packageId = String.Empty;

                // Sms authenticaiton is optional, so this flag will indicate whether the user provided a mobile number or not
                bool hasSms = false;
                if (args.Length == 6)
                {
                    hasSms = true;
                    recipientSMSNumber = args[5];
                }

                // Initialize the API 
                Console.WriteLine("Initializing API");
                ClientAPI ssApi = new ClientAPI();
                ssApi.InitialSetup(sendSafelyHost, userApiKey, userApiSecret);

                try
                {
                    // Verify the API key and Secret before continuing.  
                    // Print the authenticated user's email address to the screen if valid. 
                    String userEmail = ssApi.VerifyCredentials();
                    Console.WriteLine("Connected to SendSafely as user " + userEmail);

                    // Create a new empty package 
                    Console.WriteLine("Creating Package");
                    PackageInformation pkgInfo = ssApi.CreatePackage();
                    packageId = pkgInfo.PackageId;
                    Console.WriteLine("Created new empty package with PackageID#" + pkgInfo.PackageId);

                    // Add the recipient 
                    Console.WriteLine("Adding Recipient (Package ID#" + pkgInfo.PackageId + ")");
                    Recipient newRecipient = ssApi.AddRecipient(pkgInfo.PackageId, args[4]);

                    if (hasSms)
                    {
                        // Add the recipient SMS number for SMS authentication  
                        Console.WriteLine(recipientSMSNumber);
                        Console.WriteLine("Adding Recipient Phone Number (Recipient ID#" + newRecipient.RecipientId + ")");
                        ssApi.AddRecipientPhoneNumber(pkgInfo.PackageId, newRecipient.RecipientId, recipientSMSNumber, CountryCodes.CountryCode.US);
                    }

                    // Attach/upload the file.  The ProgressCallback class will print progress updates to the screen
                    // as they are recieved.  If the file is small and the upload happens quickly, there may be no updates to print. 
                    Console.WriteLine("Uploading File");
                    File addedFile = ssApi.EncryptAndUploadFile(pkgInfo.PackageId, pkgInfo.KeyCode, fileToUpload, new ProgressCallback());
                    Console.WriteLine("Upload Complete - File Id#" + addedFile.FileId + ".  Finalizing Package.");

                    // Package is finished, call the finalize method to make the package available for pickup and print the URL for access.
                    String packageLink = ssApi.FinalizePackage(pkgInfo.PackageId, pkgInfo.KeyCode);
                    Console.WriteLine("Success: " + packageLink);

                    // Download the file again.
                    PackageInformation pkgToDownload = ssApi.GetPackageInformationFromLink(packageLink);
                    foreach(File file in pkgToDownload.Files) {
                        System.IO.FileInfo downloadedFile = ssApi.DownloadFile(pkgToDownload.PackageId, file.FileId, pkgToDownload.KeyCode, new ProgressCallback());
            	        Console.WriteLine("Downloaded File to path: " + downloadedFile.FullName);
                    }
                }
                catch (SendSafely.Exceptions.BaseException ex)
                {
                    // Catch any custom SendSafelyAPI exceptions and determine how to properly handle them
                    if (ex is SendSafely.Exceptions.FileUploadException || ex is SendSafely.Exceptions.InvalidEmailException || ex is SendSafely.Exceptions.InvalidPhonenumberException || ex is SendSafely.Exceptions.InvalidRecipientException || ex is SendSafely.Exceptions.PackageFinalizationException || ex is SendSafely.Exceptions.ApproverRequiredException)
                    {
                        // These exceptions indicate a problem that occurred during package preparation.  
                        // If a package was created, delete it so it does not remain in the user's incomplete pacakge list.  
                        Console.WriteLine("Error: " + ex.Message);
                        if (!String.IsNullOrEmpty(packageId))
                        {
                            ssApi.DeleteTempPackage(packageId);
                            Console.WriteLine("Deleted Package - Id#:" + packageId);
                        }
                    }
                    else
                    {
                        // Throw the exception if it was not one of the specific ones we handled by deleting the package. 
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}
