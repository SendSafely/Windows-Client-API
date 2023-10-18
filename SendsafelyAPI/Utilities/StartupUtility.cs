﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using SendSafely.Exceptions;
using System.Net;
using SendSafely.Objects;

namespace SendSafely.Utilities
{
    internal class StartupUtility
    {
        private Connection connection;

        public StartupUtility(String host, String privateKey, String apiKey)
        {
            this.connection = new Connection(host, privateKey, apiKey);
        }

        public StartupUtility(Connection connection)
        {
            this.connection = connection;
        }

        public StartupUtility(String host, String privateKey, String apiKey, WebProxy proxy)
        {
            if (privateKey == null || privateKey.Length == 0)
            {
                throw new InvalidCredentialsException("The private key can't be null or empty");
            }
            else if (apiKey == null || apiKey.Length == 0)
            {
                throw new InvalidCredentialsException("The API key can't be null or empty");
            }

            this.connection = new Connection(host, privateKey, apiKey, proxy);
        }

        public Objects.Version VerifyVersion()
        {
            Endpoint p = ConnectionStrings.Endpoints["version"].Clone();
            String version = "0.3";
            p.Path = p.Path.Replace("{version}", version);

            VersionResponse response = connection.Send<VersionResponse>(p);
            return response.Version;
        }

        public String VerifyCredentials()
        {
            Endpoint p = ConnectionStrings.Endpoints["verifyCredentials"].Clone();

            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new InvalidCredentialsException("Failed to verify the credentials.");
            }

            return response.Message;
        }

        public User GetUserInformation()
        {
            Endpoint p = ConnectionStrings.Endpoints["userInformation"].Clone();

            UserInformationResponse response = connection.Send<UserInformationResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new ActionFailedException(response.Response.ToString(), response.Message);
            }

            return Convert(response);
        }

        public Connection GetConnectionObject()
        {
            return this.connection;
        }

        private User Convert(UserInformationResponse response)
        {
            User user = new User();
            user.AllowPublicKey = response.AllowPublicKey;
            user.ClientKey = response.ClientKey;
            user.Email = response.Email;
            user.FirstName = response.FirstName;
            user.Id = response.Id;
            user.LastName = response.LastName;
            user.PackageLife = response.PackageLife;
            return user;
        }
    }
}
