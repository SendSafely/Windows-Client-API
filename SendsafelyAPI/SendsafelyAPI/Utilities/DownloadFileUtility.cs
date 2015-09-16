using System;
using System.Collections.Generic;
using System.Text;
using SendSafely.Exceptions;
using System.IO;
using SendSafely.Objects;

namespace SendSafely.Utilities
{
    class DownloadFileUtility
    {
        private PackageInformation pkgInfo;
        private ISendSafelyProgress progress;
        private Connection connection;
        private String downloadAPI;
        private int BUFFER_SIZE = 1024;

        public DownloadFileUtility(Connection connection, PackageInformation pkgInfo, ISendSafelyProgress progress, String downloadAPI)
        {
            this.pkgInfo = pkgInfo;
            this.progress = progress;
            this.connection = connection;
            this.downloadAPI = downloadAPI;
        }

        public FileInfo downloadFile(String fileId)
        {
	        File fileToDownload = findFile(fileId);
		
	        FileInfo newFile = createTempFile(fileToDownload);

            Endpoint p = createEndpoint(pkgInfo, fileId);
            using (FileStream decryptedFileStream = newFile.OpenWrite())
            {
                for (int i = 1; i <= fileToDownload.Parts; i++)
                {
                    FileInfo tmpFile = createTempFile();
                    using (FileStream segmentStream = tmpFile.OpenWrite())
                    {
                        using (ProgressStream progressStream = new ProgressStream(segmentStream, progress, "Downloading", fileToDownload.FileSize, 0))
                        {
                            DownloadSegment(progressStream, p, i);
                        }
                    }
                    String dataToDecrypt = System.IO.File.ReadAllText(tmpFile.FullName);
                    using(FileStream segmentStream = tmpFile.OpenRead())
                    {
                        DecryptFile(segmentStream, decryptedFileStream);
                    }
                }
            }

            return newFile;
	    }

        private Endpoint createEndpoint(PackageInformation pkgInfo, String fileId)
        {
            Endpoint p = ConnectionStrings.Endpoints["downloadFile"].Clone();
            p.Path = p.Path.Replace("{packageId}", pkgInfo.PackageId);
            p.Path = p.Path.Replace("{fileId}", fileId);
            return p;
        }

        private void DecryptFile(Stream encryptedFile, Stream decryptedFile)
        {
            CryptUtility cu = new CryptUtility();
            cu.DecryptFile(decryptedFile, encryptedFile, getDecryptionKey());
        }

        private void DownloadSegment(Stream progressStream, Endpoint p, int part)
        {
            DownloadFileRequest request = new DownloadFileRequest();
            request.Api = this.downloadAPI;
            request.Checksum = createChecksum();
            request.Part = part;

            using (Stream objStream = connection.CallServer(p, request))
            {
                using (StreamReader objReader = new StreamReader(objStream))
                {
                    byte[] tmp = new byte[BUFFER_SIZE];
                    int l;

                    while ((l = objStream.Read(tmp, 0, BUFFER_SIZE)) != 0)
                    {
                        progressStream.Write(tmp, 0, l);
                    }
                }
            }
        }

        private char[] getDecryptionKey()
        {
            String keyString = pkgInfo.ServerSecret + pkgInfo.KeyCode;
            return keyString.ToCharArray();
        }

        private String createChecksum()
        {
            CryptUtility cu = new CryptUtility();
            return cu.pbkdf2(pkgInfo.KeyCode, pkgInfo.PackageCode, 1024);
        }

        private File findFile(String fileId)
	    {
		    foreach(File f in pkgInfo.Files) {
			    if(f.FileId.Equals(fileId)) {
				    return f;
			    }
		    }
            throw new FileDownloadException("Failed to find the file");
	    }

        private FileInfo createTempFile(File file)
	    {
            return createTempFile(Guid.NewGuid().ToString());
	    }

        private FileInfo createTempFile()
        {
            return createTempFile(Guid.NewGuid().ToString());
        }

        private FileInfo createTempFile(String fileName)
        {
            return new FileInfo(System.IO.Path.GetTempPath() + fileName);
        }
    }
}
