using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * <p>
     * Represents an KS3 Access Control List (ACL), including the ACL's set of
     * grantees and the permissions assigned to each grantee.
     * </p>
     * <p>
     * Each bucket and object in KS3 has an ACL that defines its access
     * control policy. When a request is made, KS3 authenticates the request
     * using its standard authentication procedure and then checks the ACL to verify
     * the sender was granted access to the bucket or object. If the sender is
     * approved, the request proceeds. Otherwise, KS3 returns an error.
     * </p>
     * <p>
     * An ACL contains a list of grants. Each grant consists of one grantee and one
     * permission. ACLs only grant permissions; they do not deny them.
     * </p>
     * <p>
     * For convenience, some commonly used ACLs are defined in
     * {@link CannedAccessControlList}.
     * </p>
     * <p>
     * Note: Bucket and object ACLs are completely independent; an object does not
     * inherit an ACL from its bucket. For example, if you create a bucket and
     * grant write access to another user, you will not be able to access the user's
     * objects unless the user explicitly grants access. This also applies if you
     * grant anonymous write access to a bucket. Only the user "anonymous" will be
     * able to access objects the user created unless permission is explicitly
     * granted to the bucket owner.
     * </p>
     * <p>
     * Important: Do not grant the anonymous group
     * write access to buckets, as you will have no control over the objects
     * others can store and their associated charges. For more information, see
     * {@link Grantee} and {@link Permissions}.
     * </p>
     *
     * @see CannedAccessControlList
     */
    public class AccessControlList
    {
        private HashSet<Grant> grants = new HashSet<Grant>();
        private Owner owner = null;

        /**
         * Gets the owner of the {@link AccessControlList}.
         * <p>
         * Every bucket and object in Amazon S3 has an owner, the user that created
         * the bucket or object. The owner of a bucket or object cannot be changed.
         * However, if the object is overwritten by another user (deleted and
         * rewritten), the new object will have a new owner.
         * </p>
         * <p>
         * Note: Even the owner is subject to the access control list (ACL). For example, if an owner does
         * not have {@link Permission#Read} access to an object, the owner cannot
         * read that object. However, the owner of an object always has write access
         * to the access control policy ({@link Permission#WriteAcp}) and can change
         * the ACL to read the object.
         * </p>
         */
        public Owner getOwner()
        {
            return owner;
        }

        /**
         * For internal use only. Sets the owner on this access control list (ACL). This method is only intended for internal use
         * by the library. The owner of a bucket or object cannot be changed.
         * However the object can be overwritten by the new desired owner (deleted
         * and rewritten).
         */
        public void setOwner(Owner owner)
        {
            this.owner = owner;
        }

        /**
         * Adds a grantee to the access control list (ACL) with the given permission. 
         * If this access control list already
         * contains the grantee (i.e. the same grantee object) the permission for the
         * grantee will be updated.
         */
        public void grantPermission(Grantee grantee, String permission)
        {
            this.grants.Add(new Grant(grantee, permission));
        }

        /**
         * Adds a set of grantee/permission pairs to the access control list (ACL), where each item in the
         * set is a {@link Grant} object.
         */
        public void grantAllPermissions(List<Grant> grantList)
        {
            foreach (Grant grant in grantList)
                this.grantPermission(grant.getGrantee(), grant.getPermission());
        }

        /**
         * Revokes the permissions of a grantee by removing the grantee from the access control list (ACL).
         */
        public void revokeAllPermissions(Grantee grantee)
        {
            List<Grant> grantsToRemove = new List<Grant>();
            foreach (Grant grant in grants)
                if (grant.getGrantee().Equals(grantee))
                    grantsToRemove.Add(grant);

            foreach (Grant grant in grantsToRemove)
                grants.Remove(grant);
        }

        /**
         * Gets the set of {@link Grant} objects in this access control list (ACL).
         */
        public HashSet<Grant> getGrants()
        {
            return grants;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("AccessControlList:");
            builder.Append("\nOwner:\n" + this.owner);
            builder.Append("\nGrants:");

            foreach (Grant grant in grants)
                builder.Append("\n" + grant);

            return builder.ToString();
        }
    }
}
