using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class CreateBucketRequest : KS3Request
    {
        /** The name of the KS3 bucket to create. */
        private String bucketName;

        /**
         * An optional access control list to apply to the new bucket.
         */
        private CannedAccessControlList cannedAcl;

        public CreateBucketRequest(String bucketName)
        {
            this.bucketName = bucketName;
        }

        /**
         * Sets the name of the KS3 bucket to create.
         */
        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

        /**
         * Gets the name of the KS3 bucket to create.
         */
        public String getBucketName()
        {
            return this.bucketName;
        }

        /**
         * Returns the optional access control list for the new bucket.
         */
        public CannedAccessControlList getCannedAcl()
        {
            return this.cannedAcl;
        }

        /**
         * Sets the optional access control list for the new bucket. If specified,
         * cannedAcl will be ignored.
         */
        public void setCannedAcl(CannedAccessControlList cannedAcl)
        {
            this.cannedAcl = cannedAcl;
        }
    }
}
