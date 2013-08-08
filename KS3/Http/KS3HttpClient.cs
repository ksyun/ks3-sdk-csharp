using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using KS3.Auth;
using KS3.Model;
using KS3.Internal;
using KS3.Transform;
using KS3.KS3Exception;

namespace KS3.Http
{
    public class KS3HttpClient
    {
        /** Client configuration options, such as proxy settings, max retries, etc. */
        private ClientConfiguration config;

        private HttpRequestFactory httpRequestFactory = new HttpRequestFactory();

        private ErrorResponseHandler errorResponseHandler = new ErrorResponseHandler(new ErrorResponseUnmarshaller());

        public KS3HttpClient(ClientConfiguration clientConfiguration)
        {
            this.config = clientConfiguration;
            this.init();
        }

        private void init()
        {
            /* Set max connections */
            int maxConnections = config.getMaxConnections();
            ServicePointManager.DefaultConnectionLimit = maxConnections;

            /* Set proxy if configured */
            String proxyHost = config.getProxyHost();
            int proxyPort = config.getProxyPort();
            if (proxyHost != null && proxyPort > 0)
            {
                WebProxy webProxy = new WebProxy(proxyHost, proxyPort);

                String proxyUsername = config.getProxyUsername();
                String proxyPassword = config.getProxyPassword();
                if (proxyUsername != null && proxyPassword != null)
                {
                    NetworkCredential credential = new NetworkCredential(proxyUsername, proxyPassword);
                    webProxy.Credentials = credential;
                }

                WebRequest.DefaultWebProxy = webProxy;
            }
            else
                WebRequest.DefaultWebProxy = null;
        }

        public X excute<X, Y>(Request<Y> request, HttpResponseHandler<X> responseHandler, KS3Signer<Y> ks3Signer) where Y : KS3Request
        {
            this.setUserAgent(request);

            int retryCount = 0;
            Uri redirectedURI = null;

            // Make a copy of the original request params and headers so that we can
            // permute it in this loop and start over with the original every time.
            Dictionary<String, String> originalParameters = new Dictionary<String, String>(request.getParameters());
            Dictionary<String, String> originalHeaders = new Dictionary<String, String>(request.getHeaders());

            for (; ; )
            {
                if (retryCount > 0)
                {
                    request.setParameters(originalParameters);
                    request.setHeaders(originalHeaders);
                    if (request.getContent() != null) request.getContent().Seek(0, new SeekOrigin());
                }

                HttpWebRequest httpRequest = null;
                HttpWebResponse httpResponse = null;

                try
                {
                    // Sign the request if a signer was provided
                    if (ks3Signer != null && request.getOriginalRequest().getRequestCredentials() != null)
                        ks3Signer.sign(request, request.getOriginalRequest().getRequestCredentials());

                    httpRequest = httpRequestFactory.createHttpRequest(request, this.config, redirectedURI);
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                    if (isRequestSuccessful(httpResponse))
                        return responseHandler.handle(httpResponse);
                    else if (isTemporaryRedirect(httpResponse))
                        redirectedURI = new Uri(httpResponse.Headers["location"]);
                }
                catch (WebException we)
                {
                    HttpWebResponse errorResponse = (HttpWebResponse)we.Response;
                    ServiceException serviceException = null;
                    try
                    {
                        serviceException = errorResponseHandler.handle(errorResponse);
                    }
                    catch
                    {
                        throw we;
                    }
                    if (!shouldRetry(retryCount, serviceException)) throw serviceException;
                }
                finally
                {
                    if(httpResponse != null) httpResponse.Close();
                    ++retryCount;
                }
            }
            throw new Exception("Unable to execute HTTP request.");
        }

        /**
         * Sets a User-Agent for the specified request, taking into account
         * any custom data.
         */
        private void setUserAgent<T>(Request<T> request) where T : KS3Request
        {
            String userAgent = config.getUserAgent();
            if (userAgent != null) request.addHeader(Headers.USER_AGENT, userAgent);
        }

        private Boolean isRequestSuccessful(HttpWebResponse response)
        {
            return (int)response.StatusCode / 100 == (int)HttpStatusCode.OK / 100;
        }

        private Boolean isTemporaryRedirect(HttpWebResponse response)
        {
            return response.StatusCode == HttpStatusCode.TemporaryRedirect &&
                                          response.Headers.AllKeys.Contains("location") &&
                                          response.Headers["location"].Length > 0;
        }

        private Boolean shouldRetry(int retryCount, ServiceException serviceException)
        {
            if (retryCount >= this.config.getMaxErrorRetry()) return false;

            if (serviceException.getStatusCode() == (int)HttpStatusCode.InternalServerError
                || serviceException.getStatusCode() == (int)HttpStatusCode.ServiceUnavailable)
                return true;

            if ("RequestTimeTooSkewed".Equals(serviceException.getErrorCode())
                || "InvalidSignatureException".Equals(serviceException.getErrorCode())
                || "SignatureDoesNotMatch".Equals(serviceException.getErrorCode()))
                return true;

            return false;
        }
    }
}
