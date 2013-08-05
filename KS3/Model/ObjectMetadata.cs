using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class ObjectMetadata
    {
        /**
         * Custom user metadata, represented in responses with the x-kss-meta-
         * header prefix
         */
        private Dictionary<String, String> userMetadata = new Dictionary<String, String>();

        /**
         * All other (non user custom) headers such as Content-Length, Content-Type,
         * etc.
         */
        private Dictionary<String, Object> metadata = new Dictionary<String, Object>();

        public Dictionary<String, Object> getRawMetadata()
        {
            return this.metadata;
        }

        public Dictionary<String, String> getUserMetadata()
        {
            return this.userMetadata;
        }

        public void setUserMetadata(Dictionary<String, String> userMetadata)
        {
            this.userMetadata = userMetadata;
        }

        public void addUserMetaData(String key, String value)
        {
            this.userMetadata[key] = value;
        }

        /**
         * For internal use only. Sets a specific metadata header value. Not
         * intended to be called by external code.
         */
        public void setHeader(String key, Object value)
        {
            metadata[key] = value;
        }

        public DateTime? getLastModified()
        {
            if (!metadata.ContainsKey(Headers.LAST_MODIFIED)) return null;
            return (DateTime)metadata[Headers.LAST_MODIFIED];
        }

        public void setLastModified(DateTime lastModified)
        {
            metadata[Headers.LAST_MODIFIED] = lastModified;
        }

        public long getContentLength()
        {
            if (!metadata.ContainsKey(Headers.CONTENT_LENGTH)) return 0;
            return (long)metadata[Headers.CONTENT_LENGTH];
        }

        public void setContentLength(long contentLength)
        {
            metadata[Headers.CONTENT_LENGTH] = contentLength;
        }

        public String getContentType()
        {
            if (!metadata.ContainsKey(Headers.CONTENT_TYPE)) return null;
            return (String)metadata[Headers.CONTENT_TYPE];
        }

        public void setContentType(String contentType)
        {
            metadata[Headers.CONTENT_TYPE] = contentType;
        }

        public String getContentEncoding()
        {
            if (!metadata.ContainsKey(Headers.CONTENT_ENCODING)) return null;
            return (String)metadata[Headers.CONTENT_ENCODING];
        }

        public void setContentEncoding(String encoding)
        {
            metadata[Headers.CONTENT_ENCODING] = encoding;
        }

        public void setContentMD5(String md5Base64)
        {
            if (md5Base64 == null) metadata.Remove(Headers.CONTENT_MD5);
            else metadata[Headers.CONTENT_MD5] = md5Base64;
        }

        public String getContentMD5()
        {
            if (!metadata.ContainsKey(Headers.CONTENT_MD5)) return null;
            return (String)metadata[Headers.CONTENT_MD5];
        }

        public void setContentDisposition(String disposition)
        {
            metadata[Headers.CONTENT_DISPOSITION] = disposition;
        }

        public String getContentDisposition()
        {
            if (!metadata.ContainsKey(Headers.CONTENT_DISPOSITION)) return null;
            return (String)metadata[Headers.CONTENT_DISPOSITION];
        }

        public String getETag()
        {
            if (!metadata.ContainsKey(Headers.ETAG)) return null;
            return (String)metadata[Headers.ETAG];
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<metadata>");
            foreach(String name in this.metadata.Keys)
                builder.Append("\n" + name + ": " + metadata[name]);
            builder.Append("\n</metadata>");

            builder.Append("\n<userMetadata>");
            foreach(String name in this.userMetadata.Keys)
                builder.Append("\n" + name + ": " + userMetadata[name]);
            builder.Append("\n</userMetadata>");

            return builder.ToString();
        }
    }
}
