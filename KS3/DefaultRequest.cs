using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using KS3.Http;

namespace KS3.Model
{
    public class DefaultRequest<T> : Request<T>
    {
        /** The resource path being requested */
        private String resourcePath;

        /** Map of the parameters being sent as part of this request */
        private Dictionary<String, String> parameters = new Dictionary<String, String>();

        /** Map of the headers included in this request */
        private Dictionary<String, String> headers = new Dictionary<String, String>();

        /** The service endpoint to which this request should be sent */
        private Uri endpoint;

        /**
         * The original, user facing request object which this internal request
         * object is representing
         */
        private KS3Request originalRequest;

        /** The HTTP method to use when sending this request. */
        private HttpMethodName httpMethod = HttpMethodName.POST;

        /** An optional stream from which to read the request payload. */
        private Stream content;

        /** An optional time offset to account for clock skew */
        private int timeOffset;

        /**
         * Constructs a new DefaultRequest with the specified original, user facing request object.
         */
        public DefaultRequest(KS3Request originalRequest)
        {
            this.originalRequest = originalRequest;
        }

        /**
         * Returns the original, user facing request object which this internal
         */
        public KS3Request getOriginalRequest()
        {
            return this.originalRequest;
        }

        public void addHeader(String name, String value)
        {
            this.headers[name] = value;
        }

        public Dictionary<String, String> getHeaders()
        {
            return this.headers;
        }

        public void setResourcePath(String resourcePath)
        {
            this.resourcePath = resourcePath;
        }

        public String getResourcePath()
        {
            return this.resourcePath;
        }

        public void addParameter(String name, String value)
        {
            this.parameters[name] = value;
        }

        public Dictionary<String, String> getParameters()
        {
            return this.parameters;
        }

        public HttpMethodName getHttpMethod()
        {
            return this.httpMethod;
        }

        public void setHttpMethod(HttpMethodName httpMethod)
        {
            this.httpMethod = httpMethod;
        }

        public void setEndpoint(Uri endpoint)
        {
            this.endpoint = endpoint;
        }

        public Uri getEndpoint()
        {
            return this.endpoint;
        }

        public Stream getContent()
        {
            return content;
        }

        public void setContent(Stream content)
        {
            this.content = content;
        }

        public void setHeaders(Dictionary<String, String> headers)
        {
            this.headers = new Dictionary<String, String>(headers);
        }

        public void setParameters(Dictionary<String, String> parameters)
        {
            this.parameters = new Dictionary<String, String>(parameters);
        }

        public int getTimeOffset()
        {
            return this.timeOffset;
        }

        public void setTimeOffset(int timeOffset)
        {
            this.timeOffset = timeOffset;
        }

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(this.getHttpMethod().ToString() + " ");
            builder.Append(this.getEndpoint().ToString() + " ");

            builder.Append("/" + (this.getResourcePath() != null ? this.getResourcePath() : "") + " ");

            if (this.getParameters().Count() != 0)
            {
                builder.Append("Parameters: (");
                foreach (String key in this.getParameters().Keys)
                {
                    String value = this.getParameters()[key];
                    builder.Append(key + ": " + value + ", ");
                }
                builder.Append(") ");
            }

            if (this.getHeaders().Count() != 0)
            {
                builder.Append("Headers: (");
                foreach (String key in this.getHeaders().Keys)
                {
                    String value = this.getHeaders()[key];
                    builder.Append(key + ": " + value + ", ");
                }
                builder.Append(") ");
            }

            return builder.ToString();
        }
    }
}
