using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KS3.Model;

namespace KS3.Transform
{
    public class AclXmlFactory
    {
        private static String xmlns = "http://www.w3.org/2001/XMLSchema-instance";

        public String convertToXmlString(AccessControlList acl)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<AccessControlPolicy>");
            builder.Append(this.convertOwner(acl.getOwner()));
            builder.Append(this.convertGrants(acl.getGrants()));
            builder.Append("</AccessControlPolicy>");

            return builder.ToString();
        }

        private String convertOwner(Owner owner)
        {
            if (owner == null)
                return null;

            return "<Owner><DisplayName>" + owner.getDisplayName() + "</DisplayName><ID>" + owner.getId() + "</ID></Owner>";
        }

        private String convertGrants(HashSet<Grant> grants)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<AccessControlList>");
            foreach (Grant grant in grants)
                builder.Append(this.convertGrant(grant));
            builder.Append("</AccessControlList>");

            return builder.ToString();
        }

        private String convertGrant(Grant grant)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Grant>");
            builder.Append(this.convertGrantee(grant.getGrantee()));
            builder.Append(this.convertPermission(grant.getPermission()));
            builder.Append("</Grant>");

            return builder.ToString();
        }

        private String convertGrantee(Grantee grantee)
        {
            if (grantee.GetType().Equals(typeof(CanonicalGrantee)))
                return this.convertCanonicalGrantee((CanonicalGrantee)grantee);
            else if (grantee.GetType().Equals(typeof(GroupGrantee)))
                return this.convertGroupGrantee((GroupGrantee)grantee);

            return null;
        }

        private String convertCanonicalGrantee(CanonicalGrantee grantee)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Grantee xmlns:xsi=\"" + xmlns + "\" xsi:type=\"CanonicalUser\">");
            builder.Append("<DisplayName>" + grantee.getDisplayName() + "</DisplayName>");
            builder.Append("<ID>" + grantee.getIdentifier() + "</ID>");
            builder.Append("</Grantee>");

            return builder.ToString();
        }

        private String convertGroupGrantee(GroupGrantee grantee)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Grantee xmlns:xsi=\"" + xmlns + "\" xsi:type=\"Group\">");
            builder.Append("<URI>" + grantee.getIdentifier() + "</URI>");

            return builder.ToString();
        }

        private String convertPermission(String permission)
        {
            return "<Permission>" + permission + "</Permission>";
        }
    }
}
