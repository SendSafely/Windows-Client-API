using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Bcpg;
using SendSafely.Objects;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using SendSafely.Exceptions;

namespace SendSafely.Utilities
{
    class CryptUtility
    {
        public String GenerateToken()
        {
            byte[] randomBytes = new byte[32];
            RNGCryptoServiceProvider prng = new RNGCryptoServiceProvider();
            prng.GetBytes(randomBytes);

            return EncodingUtil.Base64Encode(randomBytes);
        }

        public void EncryptFile(FileInfo encryptedFile, FileInfo inputFile, String filename, char[] passPhrase, ISendSafelyProgress progress)
        {
            using (FileStream outStream = encryptedFile.OpenWrite())
            {
                PgpEncryptedDataGenerator cPk = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Aes256, true);
                cPk.AddMethod(passPhrase);
                using (Stream cOut = cPk.Open(outStream, new byte[1 << 16]))
                {
                    WriteFileToLiteralData(cOut, PgpLiteralData.Binary, inputFile, filename, inputFile.Length);
                }
            }
        }

        public void DecryptFile(Stream outStream, Stream inputStream, char[] passPhrase)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);

            PgpObjectFactory pgpF = new PgpObjectFactory(inputStream);
            PgpEncryptedDataList enc = null;
            PgpObject o = pgpF.NextPgpObject();

            //
            // the first object might be a PGP marker packet.
            //
            if (o is PgpEncryptedDataList)
            {
                enc = (PgpEncryptedDataList)o;
            }
            else
            {
                enc = (PgpEncryptedDataList)pgpF.NextPgpObject();
            }

            PgpPbeEncryptedData pbe = (PgpPbeEncryptedData)enc[0];

            Stream clear = pbe.GetDataStream(passPhrase);

            PgpObjectFactory pgpFact = new PgpObjectFactory(clear);

            PgpLiteralData ld = (PgpLiteralData)pgpFact.NextPgpObject();

            Stream unc = ld.GetInputStream();

            byte[] buf = new byte[1 << 16];
            int len;
            while ((len = unc.Read(buf, 0, buf.Length)) > 0)
            {
                outStream.Write(buf, 0, len);
            }

            // Finally verify the integrity
            if (pbe.IsIntegrityProtected())
            {
                if (!pbe.Verify())
                {
                    throw new MessageVerificationException("Failed to verify the message. It might have been modified in transit.");
                }
            }
        }

        public String DecryptMessage(String encryptedMessage, char[] passPhrase)
        {
            // Remove the Base64 encoding
            byte[] rawMessage = Convert.FromBase64String(encryptedMessage);

            Stream inputStream = new MemoryStream(rawMessage);

            inputStream = PgpUtilities.GetDecoderStream(inputStream);

            PgpObjectFactory pgpF = new PgpObjectFactory(inputStream);
            PgpEncryptedDataList enc = null;
            PgpObject o = pgpF.NextPgpObject();

            //
            // the first object might be a PGP marker packet.
            //
            if (o is PgpEncryptedDataList)
            {
                enc = (PgpEncryptedDataList)o;
            }
            else
            {
                enc = (PgpEncryptedDataList)pgpF.NextPgpObject();
            }

            PgpPbeEncryptedData pbe = (PgpPbeEncryptedData)enc[0];

            Stream clear = pbe.GetDataStream(passPhrase);

            PgpObjectFactory pgpFact = new PgpObjectFactory(clear);

            PgpLiteralData ld = (PgpLiteralData)pgpFact.NextPgpObject();

            Stream unc = ld.GetInputStream();

            String message;
            using (StreamReader reader = new StreamReader(unc, Encoding.UTF8))
            {
                message = reader.ReadToEnd();
            }

            // Finally verify the integrity
            if (pbe.IsIntegrityProtected())
            {
                if (!pbe.Verify())
                {
                    throw new MessageVerificationException("Failed to verify the message. It might have been modified in transit.");
                }
            } 

            return message;
        }

        public String EncryptMessage(String unencryptedMessage, char[] passPhrase)
        {
            // Convert the input to a byte array. Since it's text we will interpret it as Ascii
            byte[] unencryptedByteArray = System.Text.Encoding.ASCII.GetBytes (unencryptedMessage);
	        
	        PgpLiteralDataGenerator lData = new PgpLiteralDataGenerator();
	
            // Write the data to a literal
            MemoryStream bOut;
            using (bOut = new MemoryStream() )
            {
                using (Stream pOut = lData.Open(bOut, PgpLiteralData.Binary, PgpLiteralData.Console, unencryptedByteArray.Length, DateTime.Now ))
                {
                    pOut.Write(unencryptedByteArray, 0, unencryptedByteArray.Length);
                }
            }
            lData.Close();
	
            PgpEncryptedDataGenerator cPk = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Aes256, true);
            cPk.AddMethod(passPhrase);
	        
	        byte[] bytes = bOut.ToArray();

            MemoryStream encOut;
            using (encOut = new MemoryStream())
            {
                using (Stream cOut = cPk.Open(encOut, bytes.Length) )
                {
                    cOut.Write(bytes, 0, bytes.Length);
                }
            }

            return Convert.ToBase64String(encOut.ToArray());
        }

        public String pbkdf2(String value, String salt, int iterations)
        {
            String hash = EncodingUtil.HexEncode(PBKDF2Sha256GetBytes(
                32, 
                System.Text.Encoding.UTF8.GetBytes(value),
                System.Text.Encoding.UTF8.GetBytes(salt),
                iterations));

            return hash;
        }

        public String createSignature(String key, String data)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] hashValue;
            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                hashValue = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
            }
            return EncodingUtil.HexEncode(hashValue);
        }

        private byte[] PBKDF2Sha256GetBytes(int dklen, byte[] password, byte[] salt, int iterationCount)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(password))
            {
                int hashLength = hmac.HashSize / 8;
                if ((hmac.HashSize & 7) != 0)
                    hashLength++;
                int keyLength = dklen / hashLength;
                if ((long)dklen > (0xFFFFFFFFL * hashLength) || dklen < 0)
                    throw new ArgumentOutOfRangeException("dklen");
                if (dklen % hashLength != 0)
                    keyLength++;
                byte[] extendedkey = new byte[salt.Length + 4];
                Buffer.BlockCopy(salt, 0, extendedkey, 0, salt.Length);
                using (var ms = new System.IO.MemoryStream())
                {
                    for (int i = 0; i < keyLength; i++)
                    {
                        extendedkey[salt.Length] = (byte)(((i + 1) >> 24) & 0xFF);
                        extendedkey[salt.Length + 1] = (byte)(((i + 1) >> 16) & 0xFF);
                        extendedkey[salt.Length + 2] = (byte)(((i + 1) >> 8) & 0xFF);
                        extendedkey[salt.Length + 3] = (byte)(((i + 1)) & 0xFF);
                        byte[] u = hmac.ComputeHash(extendedkey);
                        Array.Clear(extendedkey, salt.Length, 4);
                        byte[] f = u;
                        for (int j = 1; j < iterationCount; j++)
                        {
                            u = hmac.ComputeHash(u);
                            for (int k = 0; k < f.Length; k++)
                            {
                                f[k] ^= u[k];
                            }
                        }
                        ms.Write(f, 0, f.Length);
                        Array.Clear(u, 0, u.Length);
                        Array.Clear(f, 0, f.Length);
                    }
                    byte[] dk = new byte[dklen];
                    ms.Position = 0;
                    ms.Read(dk, 0, dklen);
                    ms.Position = 0;
                    for (long i = 0; i < ms.Length; i++)
                    {
                        ms.WriteByte(0);
                    }
                    Array.Clear(extendedkey, 0, extendedkey.Length);
                    return dk;
                }
            }
        }

        private void WriteFileToLiteralData(Stream pOut, char format, FileInfo dataToRead, String filename, long fileSize)
        {
            //PGPLiteralDataGenerator lData = new PGPLiteralDataGenerator();
		    //OutputStream pOut = lData.open(out, fileType, filename, filesize, new Date());
            PgpLiteralDataGenerator lData = new PgpLiteralDataGenerator();
		    //lData.Open(pOut, format, dataToRead);

            using (Stream lOut = lData.Open(pOut, format, filename, fileSize, DateTime.Now))
            {
                using (Stream inputStream = dataToRead.OpenRead())
                {

                    byte[] buf = new byte[1 << 16];
                    int len;
                    while ((len = inputStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        lOut.Write(buf, 0, len);
                    }
                }
            }
		}
    }
}
