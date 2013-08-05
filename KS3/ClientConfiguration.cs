using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KS3.Http;

namespace KS3
{
    public class ClientConfiguration
    {
        /*
         * The default time-out value in milliseconds for the GetResponse and 
         * GetRequestStream methods of the HttpWebRequest.
         */
        public static int DEFAULT_TIMEOUT = 60 * 1000;

        /** The default timeout for a connected socket. */
        public static int DEFAULT_READ_WRITE_TIMEOUT = 15 * 60 * 1000;

        /** The default HTTP user agent header. */
        public static String DEFAULT_USER_AGENT = "KS3 User";

        /** The default maximum number of retries for error responses. */
        public static int DEFAULT_MAX_RETRIES = 3;

        /** The HTTP user agent header passed with all HTTP requests. */
        private String userAgent = DEFAULT_USER_AGENT;

        /**
         * The maximum number of times that a retryable failed request (ex: a 5xx
         * response from a service) will be retried.
         */
        private int maxErrorRetry = DEFAULT_MAX_RETRIES;

        /**
         * The protocol to use when connecting to KS3.
         */
        private String protocol = Protocol.HTTP;

        /**
         * Gets or sets a time-out in milliseconds when writing to or reading from 
         * a stream.
         */
        private int readWriteTimeout = DEFAULT_READ_WRITE_TIMEOUT;

        /**
         * Gets or sets the time-out value in milliseconds for the GetResponse and 
         * GetRequestStream methods of the HttpWebRequest.
         */
        private int timeout = DEFAULT_TIMEOUT;

        public ClientConfiguration() { }

        public ClientConfiguration(ClientConfiguration other) {
            this.timeout = other.timeout;
            this.maxErrorRetry = other.maxErrorRetry;
            this.protocol = other.protocol;
            this.readWriteTimeout = other.readWriteTimeout;
            this.userAgent = other.userAgent;
        }

        public String getProtocol()
        {
            return this.protocol;
        }

        public void setProtocol(String protocol)
        {
            this.protocol = protocol;
        }

        public String getUserAgent()
        {
            return this.userAgent;
        }

        public void setUserAgent(String userAgent)
        {
            this.userAgent = userAgent;
        }

        public int getMaxErrorRetry()
        {
            return this.maxErrorRetry;
        }

        public void setMaxErrorRetry(int maxErrorRetry)
        {
            this.maxErrorRetry = maxErrorRetry;
        }

        public int getReadWriteTimeout()
        {
            return this.readWriteTimeout;
        }

        public void setReadWriteTimeout(int readWriteTimeout)
        {
            this.readWriteTimeout = readWriteTimeout;
        }

        public int getTimeout()
        {
            return this.timeout;
        }

        public void setTimeout(int timeout)
        {
            this.timeout = timeout;
        }
    }
}
