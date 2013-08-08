using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class SetObjectAclRequest : KS3Request
    {
        /** The name of the bucket whose object's ACL is being set. */
        private String bukcetName;

        /** The key of the object whose ACL is being set. */
        private String key;

        /** The custom ACL to apply to the specified bucket. */
        private AccessControlList acl;

        /** The canned ACL to apply to the specified bucket. */
        private CannedAccessControlList cannedAcl;

        public SetObjectAclRequest(String bucketName, String key, AccessControlList acl)
        {
            this.bukcetName = bucketName;
            this.key = key;
            this.acl = acl;
            this.cannedAcl = null;
        }

        public SetObjectAclRequest(String bucketName, String key, CannedAccessControlList cannedAcl)
        {
            this.bukcetName = bucketName;
            this.key = key;
            this.acl = null;
            this.cannedAcl = cannedAcl;
        }

        public String getBucketName()
        {
            return this.bukcetName;
        }

        public String getKey()
        {
            return this.key;
        }

        public AccessControlList getAcl()
        {
            return this.acl;
        }

        public CannedAccessControlList getCannedAcl()
        {
            return this.cannedAcl;
        }
    }
}
