using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class GetObjectMetadataRequest : KS3Request
    {
        /**
         * The name of the bucket containing the object's whose metadata is being
         * retrieved.
         */
        private String bucketName;

        /**
         * The key of the object whose metadata is being retrieved.
         */
        private String key;

        public GetObjectMetadataRequest(String bucketName, String key)
        {
            this.bucketName = bucketName;
            this.key = key;
        }

        public String getBucketName()
        {
            return this.bucketName;
        }

        public void setBucketname(String bucketName)
        {
            this.bucketName = bucketName;
        }

        public String getKey()
        {
            return this.key;
        }

        public void setKey(String key)
        {
            this.key = key;
        }
    }
}
