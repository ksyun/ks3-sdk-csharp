using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class DeleteBucketRequest : KS3Request
    {
        /**
         * The name of the KS3 bucket to delete.
         */
        private String bucketName;

        public DeleteBucketRequest(String bucketName)
        {
            this.bucketName = bucketName;
        }

        public String setBucketName(String bucketName)
        {
            return this.bucketName = bucketName;
        }

        public String getBucketName()
        {
            return this.bucketName;
        }
    }
}
