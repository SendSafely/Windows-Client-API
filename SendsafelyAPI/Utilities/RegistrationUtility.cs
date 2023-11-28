using System;
using System.Collections.Generic;
using System.Text;
using SendSafely.Objects;
using SendSafely.Exceptions;

namespace SendSafely.Utilities
{
    class RegistrationUtility
    {
        private Connection connection;

        public RegistrationUtility(Connection connection)
        {
            this.connection = connection;
        }

        public void StartRegistration(String email)
        {
            Endpoint p = ConnectionStrings.Endpoints["startRegistration"].Clone();
            StartRegistrationRequest req = new StartRegistrationRequest();
            req.Email = email;
            req.SendPin = false;

            StandardResponse response = connection.Send<StandardResponse>(p, req);

            if (response.Response == APIResponse.INVALID_EMAIL)
            {
                throw new InvalidEmailException(response.Message);
            }
            else if (response.Response == APIResponse.AUTH_FORBIDDEN)
            {
                throw new RegistrationNotAllowedException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public void StartPINRegistration(String email)
        {
            Endpoint p = ConnectionStrings.Endpoints["startRegistration"].Clone();
            StartRegistrationRequest req = new StartRegistrationRequest();
            req.Email = email;
            req.SendPin = true;

            StandardResponse response = connection.Send<StandardResponse>(p, req);

            Console.WriteLine(response.Response.ToString() + " " + response.Message);
            if (response.Response == APIResponse.INVALID_EMAIL)
            {
                throw new InvalidEmailException(response.Message);
            }
            else if (response.Response == APIResponse.AUTH_FORBIDDEN)
            {
                throw new RegistrationNotAllowedException(response.Message);
            }
            else if (response.Response == APIResponse.TWO_FA_ENFORCED)
            {
                throw new TwoFAEnforcedException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }
        }

        public APICredential FinishPINRegistration(String email, String pincode, String password, String secretQuestion, String answer, String firstName, String lastName, String keyDescription)
        {
            Endpoint p = ConnectionStrings.Endpoints["finishPINRegistration"].Clone();

            FinishRegistrationRequest req = new FinishRegistrationRequest();
            req.Email = email;
            req.PinCode = pincode;
            req.Password = password;
            req.Question = secretQuestion;
            req.Answer = answer;
            req.FirstName = firstName;
            req.LastName = lastName;

            StandardResponse response = connection.Send<StandardResponse>(p, req);

            if (response.Response == APIResponse.PASSWORD_COMPLEXITY)
            {
                throw new InsufficientPasswordComplexityException(response.Message);
            }
            else if (response.Response == APIResponse.TOKEN_EXPIRED)
            {
                throw new TokenExpiredException(response.Message);
            }
            else if (response.Response == APIResponse.PIN_RESEND)
            {
                throw new PINRefreshException(response.Message);
            }
            else if (response.Response == APIResponse.INVALID_TOKEN)
            {
                throw new InvalidTokenException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            p = ConnectionStrings.Endpoints["generateKey"].Clone();
            GenerateKeyRequest keyReq = new GenerateKeyRequest();
            keyReq.Email = email;
            keyReq.Password = password;
            keyReq.KeyDescription = keyDescription;

            GenerateAPIKeyResponse response2 = connection.Send<GenerateAPIKeyResponse>(p, keyReq);

            if (response2.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response2.Response.ToString(), response2.Message);
            }

            APICredential key = new APICredential();
            key.APIKey = response2.APIKey;
            key.APISecret = response2.APISecret;

            return key;
        }

        public APICredential FinishRegistration(String validationLink, String password, String secretQuestion, String answer, String firstName, String lastName, String keyDescription)
        {
            Endpoint p = ConnectionStrings.Endpoints["finishRegistration"].Clone();
            p.Path = p.Path.Replace("{token}", parseValidationLink(validationLink));

            FinishRegistrationRequest req = new FinishRegistrationRequest();
            req.Password = password;
            req.Question = secretQuestion;
            req.Answer = answer;
            req.FirstName = firstName;
            req.LastName = lastName;

            StandardResponse response = connection.Send<StandardResponse>(p, req);

            if (response.Response == APIResponse.PASSWORD_COMPLEXITY)
            {
                throw new InsufficientPasswordComplexityException(response.Message);
            }
            else if (response.Response == APIResponse.TOKEN_EXPIRED)
            {
                throw new TokenExpiredException(response.Message);
            }
            else if (response.Response == APIResponse.INVALID_TOKEN)
            {
                throw new InvalidTokenException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            // Generate the API Key.
            String email = response.Message;

            p = ConnectionStrings.Endpoints["generateKey"].Clone();
            GenerateKeyRequest keyReq = new GenerateKeyRequest();
            keyReq.Email = email;
            keyReq.Password = password;
            keyReq.KeyDescription = keyDescription;

            GenerateAPIKeyResponse response2 = connection.Send<GenerateAPIKeyResponse>(p, keyReq);

            if (response2.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response2.Response.ToString(), response2.Message);
            }

            APICredential key = new APICredential();
            key.APIKey = response2.APIKey;
            key.APISecret = response2.APISecret;

            return key;
        }

        public APICredential OAuthGenerateAPIKey(String oauthToken, String keyDescription)
        {
            Endpoint p = ConnectionStrings.Endpoints["oauthRegistration"].Clone();
            p.Path = p.Path.Replace("{token}", oauthToken);

            FinishRegistrationRequest req = new FinishRegistrationRequest();
            req.KeyDescription = keyDescription;

            GenerateAPIKeyResponse response = connection.Send<GenerateAPIKeyResponse>(p, req);

            if (response.Response == APIResponse.USER_ALREADY_EXISTS)
            {
                throw new DuplicateUserException(response.Message);
            }
            else if (response.Response == APIResponse.AUTH_FORBIDDEN)
            {
                throw new RegistrationNotAllowedException(response.Message);
            }
            else if (response.Response == APIResponse.AUTHENTICATION_FAILED)
            {
                throw new InvalidCredentialsException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            APICredential key = new APICredential();
            key.APIKey = response.APIKey;
            key.APISecret = response.APISecret;

            return key;
        }

        public APICredential GenerateAPIKey(String username, String password, String keyDescription)
        {
            Endpoint p = ConnectionStrings.Endpoints["generateKey"].Clone();
            GenerateKeyRequest keyReq = new GenerateKeyRequest();
            keyReq.Email = username;
            keyReq.Password = password;
            keyReq.KeyDescription = keyDescription;

            GenerateAPIKeyResponse response = connection.Send<GenerateAPIKeyResponse>(p, keyReq);

            if (response.Response == APIResponse.TWO_FA_REQUIRED)
            {
                throw new TwoFactorAuthException(response.Message, response.TwoFaType);
            }
            else if (response.Response == APIResponse.AUTHENTICATION_FAILED)
            {
                throw new InvalidCredentialsException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            APICredential key = new APICredential();
            key.APIKey = response.APIKey;
            key.APISecret = response.APISecret;

            return key;
        }

        public APICredential GenerateAPIKey2FA(String validationToken, String smsCode, String keyDescription)
        {
            Endpoint p = ConnectionStrings.Endpoints["generateKey2FA"].Clone();
            p.Path = p.Path.Replace("{token}", validationToken);

            GenerateKeyRequest req = new GenerateKeyRequest();
            req.SMSCode = smsCode;
            req.KeyDescription = keyDescription;

            GenerateAPIKeyResponse response = connection.Send<GenerateAPIKeyResponse>(p, req);

            if (response.Response == APIResponse.AUTHENTICATION_FAILED)
            {
                throw new InvalidCredentialsException(response.Message);
            }
            else if (response.Response == APIResponse.PIN_RESEND)
            {
                throw new PINRefreshException(response.Message);
            }
            else if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            APICredential key = new APICredential();
            key.APIKey = response.APIKey;
            key.APISecret = response.APISecret;

            return key;
        }

        private String parseValidationLink(String link)
        {
            Uri myUri = new Uri(link);
            String validationToken = System.Web.HttpUtility.ParseQueryString(myUri.Query).Get("validationLink");

            if (validationToken == null)
            {
                throw new InvalidLinkException("The validation link could not be found");
            }

            return validationToken;
        }

    }
}
