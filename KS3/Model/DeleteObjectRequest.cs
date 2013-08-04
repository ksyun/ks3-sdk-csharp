using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class DeleteObjectRequest : KS3Request
    {
        /**
         * The name of the KS3 bucket containing the object to delete.
         */
        private String bucketName;

        /**
         * The key of the object to delete.
         */
        private String key;

        public DeleteObjectRequest(String bucketName, String key)
        {
            this.bucketName = bucketName;
            this.key = key;
        }

        public String getBucketName()
        {
            return this.bucketName;
        }

        public void setBucketName(String bucketName)
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
