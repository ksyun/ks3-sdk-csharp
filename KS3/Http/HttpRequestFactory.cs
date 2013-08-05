using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using KS3.Model;
using KS3.Internal;

namespace KS3.Http
{
    public class HttpRequestFactory
    {
        private static String DEFAULT_ENCODING = "UTF-8";

        public HttpWebRequest createHttpRequest<T>(Request<T> request, ClientConfiguration clientConfiguration, Uri redirectURI) where T : KS3Request
        {
            Uri endpoint = request.getEndpoint();
            String uri = endpoint.ToString();
            if (request.getResourcePath() != null && request.getResourcePath().Length > 0)
            {
                if (request.getResourcePath().StartsWith("/"))
                {
                    if (uri.EndsWith("/")) uri = uri.Substring(0, uri.Length - 1);
                }
                else if (!uri.EndsWith("/")) uri += "/";
                uri += request.getResourcePath();
            }
            else if (!uri.EndsWith("/")) uri += "/";

            String encodedParams = this.encodeParameters(request);
            uri += encodedParams;

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpRequest.Method = request.getHttpMethod().ToString();
            
            httpRequest.AllowWriteStreamBuffering = false; // important
            httpRequest.Timeout = clientConfiguration.getTimeout();
            httpRequest.ReadWriteTimeout = clientConfiguration.getReadWriteTimeout();

            configureHeaders(httpRequest, request, clientConfiguration);
            
            if (request.getContent() != null)
            {
                Stream inputStream = request.getContent();
                Stream requestStream = httpRequest.GetRequestStream();
                int SIZE = Constants.DEFAULT_STREAM_BUFFER_SIZE;
                byte[] buf = new byte[SIZE];

                for (; ; )
                {
                    int size = inputStream.Read(buf, 0, Constants.DEFAULT_STREAM_BUFFER_SIZE);
                    if (size <= 0) break;
                    requestStream.Write(buf, 0, size);
                }
            }

            return httpRequest;
        }

        /**
         * Creates an encoded query string from all the parameters in the specified
         * request.
         */
        private String encodeParameters<T>(Request<T> request) where T : KS3Request
        {
            StringBuilder builder = new StringBuilder();
            char separator = '?';

            foreach (String name in request.getParameters().Keys)
            {
                String value = request.getParameters()[name];
                builder.Append(separator);
                builder.Append(name + (value != null ? ("=" + value) : ""));

                separator = '&';
            }

            return builder.ToString();
        }

        /** Configures the headers in the specified HTTP request. */
        private void configureHeaders<T>(HttpWebRequest httpRequest, Request<T> request, ClientConfiguration clientConfiguration) where T : KS3Request
        {
            //Uri endpoint = request.getEndpoint();
            //String hostHeader = endpoint.Host;
            //httpRequest.Host = hostHeader;

            // Copy over any other headers already in our request
            foreach (String name in request.getHeaders().Keys)
            {
                if (name.Equals(Headers.HOST)) continue;
                String value = request.getHeaders()[name];

                if (name.Equals(Headers.CONTENT_TYPE)) httpRequest.ContentType = value;
                else if (name.Equals(Headers.CONTENT_LENGTH)) httpRequest.ContentLength = int.Parse(value);
                else if (name.Equals(Headers.USER_AGENT)) httpRequest.UserAgent = value;
                else if (name.Equals(Headers.DATE)) httpRequest.Date = DateTime.Parse(value);
                else if (name.Equals(Headers.RANGE))
                {
                    String[] range = value.Split('-');
                    httpRequest.AddRange(int.Parse(range[0]), int.Parse(range[1]));
                }
                else if (name.Equals(Headers.GET_OBJECT_IF_MODIFIED_SINCE)) httpRequest.IfModifiedSince = DateTime.Parse(value);
                else httpRequest.Headers[name] = value;
            }

            /* Set content type and encoding */
            if (!httpRequest.Headers.AllKeys.Contains(Headers.CONTENT_TYPE) || httpRequest.Headers[Headers.CONTENT_TYPE].Length == 0)
                httpRequest.ContentType = Mimetypes.DEFAULT_MIMETYPE;
        }

    }
}
