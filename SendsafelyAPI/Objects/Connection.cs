using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using SendSafely.Utilities;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using SendSafely.Exceptions;
using System.Threading;
using System.Reflection;
using System.Runtime.Versioning;

namespace SendSafely.Objects
{
    internal class Connection
    {
        private String apiKey;
        private String privateKey;
        private WebProxy proxy = null;
        private Dictionary<String, String> keycodes;
        private String url;
        private String apiKeyHeadervalue = "ss-api-key";
        private String apiTimestampHeadervalue = "ss-request-timestamp";
        private String apiSignatureHeadervalue = "ss-request-signature";
        private String acceptLanguageHeaderValue = "Accept-Language";
        private String apiIntegrationTypeHeadervalue = "ss-request-api";
        private String outlookVersion = null;
        private String requestAPI = null;
        private String locale = "en-US";

        #region Constructors

        public Connection(String host, String privateKey, String apiKey)
        {
            Initialize(host, privateKey, apiKey);
        }

        public Connection(String host, String privateKey, String apiKey, WebProxy proxy)
        {
            Initialize(host, privateKey, apiKey);
            this.proxy = proxy;
        }

        public Connection(String host, WebProxy proxy)
        {
            Initialize(host, null, null);
            this.proxy = proxy;
        }

        #endregion

        #region Public Functions

        public String OutlookVersion
        {
            get { return outlookVersion; }
            set { outlookVersion = value; }
        }

        public String RequestAPI
        {
            get { return requestAPI; }
            set { requestAPI = value; }
        }

        public String ApiHost
        {
            get { return url; }
        }

        public String GetKeycode(String packageId)
        {
            if (packageId == null || !keycodes.ContainsKey(packageId))
            {
                throw new InvalidPackageException("Unknown Package Id");
            }

            return keycodes[packageId];
        }

        public void AddKeycode(String packageId, String keycode)
        {
            if (keycodes.ContainsKey(packageId))
            {
                keycodes.Remove(packageId);
            }
            keycodes.Add(packageId, keycode);
        }

        public T Send<T>(Endpoint p)
        {
            return Send<T>(p, null);
        }

        public T Send<T>(Endpoint p, Object requestObj)
        {
            String respStr;
            try
            {
                respStr = SendRequest(p, requestObj);
            }
            catch (WebException e)
            {
                // Server is not reachable..
                throw new ServerUnavailableException("Failed to connect to server.");
            }

            // We'll parse it as a StandardResponse first so we can check for auth failures.
            StandardResponse authResp = JsonConvert.DeserializeObject<StandardResponse>(respStr);
            if (authResp.Response == APIResponse.AUTHENTICATION_FAILED)
            {
                throw new InvalidCredentialsException(authResp.Message);
            }
            else if (authResp.Response == APIResponse.LIMIT_EXCEEDED)
            {
                throw new LimitExceededException(authResp.Message);
            }
            else if (authResp.Response == APIResponse.UNKNOWN_PACKAGE)
            {
                throw new InvalidPackageException(authResp.Message);
            }

            // Parse the response as T.
            T resp = JsonConvert.DeserializeObject<T>(respStr);
            return resp;
        }

        public HttpWebRequest GetRequestforFileUpload(Endpoint p, String boundary, UploadFileRequest requestData)
        {
            return GetRequestforFileUpload(p, boundary, null, requestData);
        }

        public HttpWebRequest GetRequestforFileUpload(Endpoint p, String boundary, String fileId, UploadFileRequest requestData)
        {
            String url = this.url + p.Path;

            HttpWebRequest wrReq;
            wrReq = (HttpWebRequest)WebRequest.Create(url);
            wrReq.Timeout = Timeout.Infinite;
            wrReq.Headers.Add(apiKeyHeadervalue, apiKey);

            DateTime now = DateTime.UtcNow;
            String dateStr = now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+0000";
            String signature = CreateSignature(privateKey, apiKey, p.Path, dateStr, ConvertToJSON(requestData));

            wrReq.Headers.Add(apiSignatureHeadervalue, signature);
            wrReq.Headers.Add(apiTimestampHeadervalue, dateStr);
            wrReq.Headers.Add(acceptLanguageHeaderValue, locale);

            wrReq.Method = p.Method.ToString();
            wrReq.ContentType = p.ContentType + "; boundary=" + boundary;

            String userAgent = generateUserAgent();
            wrReq.UserAgent = userAgent;

            if (proxy != null)
            {
                wrReq.Proxy = proxy;
            }

            return wrReq;
        }

        public Stream CallServer(Endpoint p, Object request)
        {
            String url = this.url + p.Path;
            HttpWebRequest wrReq;
            wrReq = (HttpWebRequest)WebRequest.Create(url);
            wrReq.Timeout = Timeout.Infinite;

            if (apiKey != null)
            {
                wrReq.Headers.Add(apiKeyHeadervalue, apiKey);
            }
            wrReq.Method = p.Method.ToString();

            wrReq.UserAgent = generateUserAgent();

            String requestString = "";
            if (request != null)
            {
                requestString = ConvertToJSON(request);
            }

            DateTime now = DateTime.UtcNow;
            String dateStr = now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "+0000";

            if (apiKey != null && privateKey != null)
            {
                String signature = CreateSignature(privateKey, apiKey, p.Path, dateStr, requestString);
                wrReq.Headers.Add(apiSignatureHeadervalue, signature);
            }
            wrReq.Headers.Add(apiTimestampHeadervalue, dateStr);
            wrReq.Headers.Add(acceptLanguageHeaderValue, locale);

            if (!String.IsNullOrEmpty(requestAPI))
            {
                wrReq.Headers.Add(apiIntegrationTypeHeadervalue, requestAPI);
            }

            if (proxy != null)
            {
                wrReq.Proxy = proxy;
            }

            wrReq.ContentType = p.ContentType;
            if (request != null)
            {
                WriteOutput(wrReq, requestString);
            }

            Stream objStream;
            objStream = wrReq.GetResponse().GetResponseStream();
            return objStream;
        }

        public void setLocale(String locale)
        {
            this.locale = locale;
        }

        #endregion

        #region Private Functions

        private void Initialize(String host, String privateKey, String apiKey)
        {
            this.SetTlsProtocol();
            this.apiKey = apiKey;
            this.privateKey = privateKey;

            host = host.TrimEnd("/".ToCharArray());

            this.url = host;
            this.keycodes = new Dictionary<String, String>();
        }

        private String SendRequest(Endpoint p, Object request)
        {
            using (var objStream = CallServer(p, request))
            {
                StreamReader objReader = new StreamReader(objStream);

                String sLine = "";
                String response = "";
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                    {
                        response += sLine;
                        Logger.Log(sLine);
                    }
                }

                return response;
            }
        }

        private String generateUserAgent()
        {
            String userAgent = "Custom (" + Environment.OSVersion.ToString() + ")";
            if (outlookVersion != null)
            {
                userAgent += " Outlook Version " + outlookVersion;
            }
            else
            {
                userAgent += ".NET API";
            }

            return userAgent;
        }

        private String CreateSignature(String privateKey, String apiKey, String uri, String dateStr, String requestData)
        {
            String content = apiKey + uri.Split('?')[0] + dateStr + requestData;
            Logger.Log("-" + content + "-");
            CryptUtility cu = new CryptUtility();
            return cu.createSignature(privateKey, content);
        }

        private String ConvertToJSON(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private void WriteOutput(WebRequest req, String requestString)
        {
            // Serialize the object
            byte[] reqData = System.Text.Encoding.UTF8.GetBytes(requestString);

            req.ContentLength = reqData.Length;

            using (var dataStream = req.GetRequestStream()) {
                dataStream.Write(reqData, 0, reqData.Length);
            }
        }

        private void SetTlsProtocol()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var attributes = assembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), false);
            var version = (TargetFrameworkAttribute)attributes[0];

            SecurityProtocolType flag;

            try
            {
                if (Enum.TryParse("Tls11", out flag))
                    ServicePointManager.SecurityProtocol |= flag;
                if (Enum.TryParse("Tls12", out flag))
                    ServicePointManager.SecurityProtocol |= flag;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to set TLS protocol for " + version.FrameworkDisplayName + ", enabled protocols " + GetEnabledSecurityProtocols(), e);
            }
        }

        private string GetEnabledSecurityProtocols()
        {
            return ServicePointManager.SecurityProtocol.ToString();
        }

        #endregion
    }
}
