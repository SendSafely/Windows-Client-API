using System;
using System.Collections.Generic;
using System.Text;
using SendSafely.Objects;
using SendSafely.Exceptions;
using SendSafely.Utilities;
using System.IO;

namespace SendSafely
{
    internal class PublicKeyUtility
    {
        private String KEY_EMAIL = "no-reply@sendsafely.com";

        private Connection connection;

        #region Constructors

        public PublicKeyUtility(Connection connection)
        {
            this.connection = connection;
        }
        
        #endregion

        public PrivateKey GenerateKeyPair(String description)
        {
            CryptUtility cu = new CryptUtility();
            Keypair keyPair = cu.GenerateKeyPair(KEY_EMAIL);

            String publicKeyId = UploadPublicKey(keyPair.PublicKey, description);

            PrivateKey privateKey = new PrivateKey();
            privateKey.ArmoredKey = keyPair.PrivateKey;
            privateKey.PublicKeyID = publicKeyId;

            return privateKey;
        }

        public void RevokePublicKey(String publicKeyId)
        {
            Endpoint p = ConnectionStrings.Endpoints["revokePublicKey"].Clone();
            p.Path = p.Path.Replace("{publicKeyId}", publicKeyId);
            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new RevokingKeyFailedException("Failed to revoke public key: " + response.Message);
            }
        }

        public String GetKeycode(String packageId, PrivateKey privateKey)
        {
            String publicKeyId = privateKey.PublicKeyID;
            String privateKeyStr = privateKey.ArmoredKey;
            
            Endpoint p = ConnectionStrings.Endpoints["getKeycode"].Clone();
            p.Path = p.Path.Replace("{publicKeyId}", publicKeyId);
            p.Path = p.Path.Replace("{packageId}", packageId);
            StandardResponse response = connection.Send<StandardResponse>(p);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new GettingKeycodeFailedException("Failed to get keycode: " + response.Message);
            }

            String encryptedKeycode = response.Message;

            CryptUtility cu = new CryptUtility();
            String keycode = cu.DecryptKeycode(privateKeyStr, encryptedKeycode);
            return keycode;
        }

        private String UploadPublicKey(String publicKey, String description)
        {
            Endpoint p = ConnectionStrings.Endpoints["addPublicKey"].Clone();
            AddPublicKeyRequest request = new AddPublicKeyRequest();
            request.PublicKey = publicKey;
            request.Description = description;
            AddPublicKeyResponse response = connection.Send<AddPublicKeyResponse>(p, request);

            if (response.Response != APIResponse.SUCCESS)
            {
                throw new AddingPublicKeyFailedException("Failed to upload public key: " + response.Message);
            }

            return response.ID;
        }
    }
}
