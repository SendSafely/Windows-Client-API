using System;
using System.Collections.Generic;
using System.Text;
using SendSafely.Objects;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using SendSafely.Exceptions;

namespace SendSafely.Utilities
{
    class FileUploader
    {
        private Endpoint p;
        private ISendSafelyProgress progress;
        private Connection connection;
        private String CRLF = "\r\n";
        private String CHARSET = "UTF-8";
        private int BUFFER_SIZE = 1024;
        private int UPLOAD_RETRY_ATTEMPTS = 5;
        private StandardResponse response;
        private String boundary;

        public FileUploader(Connection connection, Endpoint p, ISendSafelyProgress progress)
        {
            this.p = p;
            this.progress = progress;
            this.connection = connection;
            this.boundary = DateTime.Now.Ticks.ToString();
        }

        //public StandardResponse upload(FileInfo rawFile, String filename, String signature, long fileSize, String uploadType)
        public StandardResponse Upload(FileInfo rawFile, UploadFileRequest requestData, String filename, long uploadedSoFar, long fullFillSize)
        {
            using (FileStream fileStream = rawFile.OpenRead())
            {
                using (ProgressStream progressStream = new ProgressStream(fileStream, progress, "Uploading", fullFillSize, uploadedSoFar))
                {
                    long uploadedBytes = UploadSegment(progressStream, requestData, filename, rawFile.Length);
                }
            }
            return response;
        }

        #region Private Functions

        private long UploadSegment(ProgressStream fileStream, UploadFileRequest requestData, String filename, long segmentSize)
        {
            Logger.Log("Uploading file with size: " + segmentSize);
            long uploadedBytes = 0;
            String responseStr = "";

            bool notFinished = true;
            int tryCounter = 0;
            while (notFinished && tryCounter < UPLOAD_RETRY_ATTEMPTS)
            {
                HttpWebRequest req = null;
                try
                {
                    req = connection.GetRequestforFileUpload(p, boundary, requestData);
                    req.KeepAlive = false;

                    String suffix = "--" + boundary + "--" + CRLF;
                    String content = "";
                    content += GetParam("requestData", ConvertToJSON(requestData), boundary);

                    long contentLength = content.Length;
                    contentLength += suffix.Length;
                    contentLength += GetFileSegmentStart(filename).Length;
                    contentLength += segmentSize;
                    contentLength += CRLF.Length;
                    req.ContentLength = contentLength;

                    uploadedBytes = 0;
           
                    using (Stream dataStream = req.GetRequestStream())
                    {
                        Write(content, dataStream);

                        uploadedBytes = SendBinary(fileStream, filename, dataStream, boundary);

                        Write(suffix, dataStream);
                        dataStream.Flush();
                        notFinished = false;
                    }

                    responseStr = GetResponse(req);
                }
                catch (Exception e)
                {
                    // If an exception was thrown that means an IOException occured. If so we retry a couple of times.
                    if (req != null)
                    {
                        req.Abort();
                    }
                    tryCounter++;
                    uploadedBytes = 0;
                }
            }

            if (notFinished)
            {
                throw new FileUploadException("Multiple exceptions thrown when uploading.");
            }

            this.response = JsonConvert.DeserializeObject<StandardResponse>(responseStr);

            if (this.response == null)
            {
                throw new ActionFailedException("NULL_RESPONSE", "The server response could not be parsed");
            }

            if (this.response.Response == APIResponse.AUTHENTICATION_FAILED)
            {
                throw new InvalidCredentialsException(this.response.Message);
            }
            else if (this.response.Response == APIResponse.UNKNOWN_PACKAGE)
            {
                throw new InvalidPackageException(response.Message);
            }
            else if (this.response.Response == APIResponse.INVALID_EMAIL)
            {
                throw new InvalidEmailException(this.response.Message);
            }

            return uploadedBytes;
        }

        private String GetResponse(WebRequest req)
        {
            String response = "";
            using (Stream objStream = req.GetResponse().GetResponseStream())
            {
                using (StreamReader objReader = new StreamReader(objStream))
                {
                    String sLine = "";

                    while (sLine != null)
                    {
                        sLine = objReader.ReadLine();
                        if (sLine != null)
                        {
                            response += sLine;
                            //Logger.Log(sLine);
                        }
                    }
                }
            }

            return response;
        }

        private long SendBinary(ProgressStream input, String filename, Stream output, String boundary)
        {
            Write(GetFileSegmentStart(filename), output);
            output.Flush();

            byte[] tmp = new byte[BUFFER_SIZE];
            int l;

            // Wrap the stream to get some progress updates..
            long uploadedBytes = 0;

            while ((l = input.Read(tmp, 0, BUFFER_SIZE)) != 0)
            {
                output.Write(tmp, 0, l);
                output.Flush();
                uploadedBytes += l;
            }

            Write(CRLF, output);
            input.Flush();

            return uploadedBytes;
        }

        private String GetFileSegmentStart(string filename)
        {
            String content = "";
            content += "--" + boundary + CRLF;
            content += "Content-Disposition: form-data; name=\"textFile\"; filename=\"" + filename + "\"" + CRLF;
            content += "Content-Type: text/plain; charset=" + CHARSET + CRLF;
            content += CRLF;
            return content;
        }

        private String GetParam(String key, String value, String boundary)
        {
            String content = "";
            content += "--" + boundary + CRLF;
            content += "content-disposition: form-data; name=\"" + key + "\"" + CRLF;
            content += "content-type: text/plain; charset=" + CHARSET + CRLF;
            content += CRLF;
            content += value + CRLF;

            return content;
        }

        private void Write(String str, Stream stream)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
        }

        private String ConvertToJSON(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        #endregion

    }
}
