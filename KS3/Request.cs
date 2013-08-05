using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using KS3.Http;
using KS3.Model;

namespace KS3
{
    public interface Request<T>
    {
        /**
         * Adds the specified header to this request.
         */
        void addHeader(String name, String value);
        
        /**
         * Returns a map of all the headers included in this request.
         */
        Dictionary<String, String> getHeaders();

        /**
         * Sets all headers, clearing any existing ones.
         */
        void setHeaders(Dictionary<String, String> headers);

        /**
         * Sets the path to the resource being requested.
         */
        void setResourcePath(String path);

        /**
         * Returns the path to the resource being requested.
         */
        String getResourcePath();

        /**
         * Adds the specified request parameter to this request.
         */
        void addParameter(String name, String value);

        /**
         * Returns a map of all parameters in this request.
         */
        Dictionary<String, String> getParameters();

        /**
         * Sets all parameters, clearing any existing values.
         */
        void setParameters(Dictionary<String, String> parameters);

        /**
         * Returns the service endpoint to which this request should be sent.
         */
        Uri getEndpoint();

        /**
         * Sets the service endpoint to which this request should be sent.
         */
        void setEndpoint(Uri endpoint);

        /**
	     * Returns the HTTP method (GET, POST, etc) to use when sending this
	     * request.
	     */ 
        HttpMethodName getHttpMethod();

	    /**
	     * Sets the HTTP method (GET, POST, etc) to use when sending this request.
         */
        void setHttpMethod(HttpMethodName httMethod);
	    
        /**
	     * Returns the optional stream containing the payload data to include for
	     * this request.  Not all requests will contain payload data.
	     */
        Stream getContent();

	    /**
	     * Sets the optional stream containing the payload data to include for this
	     * request. Not all requests will contain payload data.
         */
        void setContent(Stream content);

        /**
         * Returns the original, user facing request object which this internal
         * request object is representing.
         */
        KS3Request getOriginalRequest();

        /**
         * Returns the optional value for time offset for this request.  This
         * will be used by the signer to adjust for potential clock skew.  
         * Value is in seconds, positive values imply the current clock is "fast",
         * negative values imply clock is slow.
         */
        int getTimeOffset();

        /**
         * Sets the optional value for time offset for this request.  This
         * will be used by the signer to adjust for potential clock skew.  
         * Value is in seconds, positive values imply the current clock is "fast",
         * negative values imply clock is slow.
         */
        void setTimeOffset(int timeOffset);
    }
}
