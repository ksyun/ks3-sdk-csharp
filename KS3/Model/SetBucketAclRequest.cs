using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class SetBucketAclRequest : KS3Request
    {
        /** The name of the bucket whose ACL is being set. */
        private String bukcetName;

        /** The canned ACL to apply to the specified bucket. */
        private CannedAccessControlList cannedAcl;

        public SetBucketAclRequest(String bucketName, CannedAccessControlList cannedAcl)
        {
            this.bukcetName = bucketName;
            this.cannedAcl = cannedAcl;
        }

        public String getBucketName()
        {
            return this.bukcetName;
        }

        public CannedAccessControlList getCannedAcl()
        {
            return this.cannedAcl;
        }
    }
}
