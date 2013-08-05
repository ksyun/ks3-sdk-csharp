﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3
{
    public class KS3Exception : Exception
    {
        private String requestId;

        /**
         * The KS3 error code represented by this exception (ex:
         * InvalidParameterValue).
         */
        private String errorCode;

        /** The HTTP status code that was returned with this error */
        private int statusCode;

        public KS3Exception() : base() { }

        /**
         * Constructs a new KS3ClientException with the specified message.
         */
        public KS3Exception(String message) : base(message) { }

        /**
         * Constructs a new KS3ClientException with the specified message and
         * exception indicating the root cause.
         */
        public KS3Exception(String message, Exception cause) : base(message, cause) { }

        public void setRequestId(String requestId)
        {
            this.requestId = requestId;
        }

        public String getRequestId()
        {
            return this.requestId;
        }

        public void setErrorCode(String errorCode)
        {
            this.errorCode = errorCode;
        }

        public String getErrorCode()
        {
            return this.errorCode;
        }

        /**
         * Sets the HTTP status code that was returned with this service exception.
         */
        public void setStatusCode(int statusCode)
        {
            this.statusCode = statusCode;
        }

        /**
         * Returns the HTTP status code that was returned with this service
         * exception.
         */
        public int getStatusCode()
        {
            return statusCode;
        }

        public override string ToString()
        {
            return String.Join("\n", new List<String> {
                   "KS3ServerException:",
                   "Status Code: " + this.getStatusCode(),
                   "Request ID: " + this.getRequestId(),
                   "Error Code: " + this.getErrorCode(),
                   "Error Message: " + this.Message,
                   this.StackTrace});
        }
    }
}