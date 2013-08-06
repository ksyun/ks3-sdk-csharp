using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class Grantee
    {
        private String id = null;
        private String displayName = null;

        public Grantee(String id)
        {
            this.id = id;
        }

        public Grantee(String id, String displayName)
        {
            this.id = id;
            this.displayName = displayName;
        }

        public void setIdentifier(String id)
        {
            this.id = id;
        }

        public String getIdentifier()
        {
            return this.id;
        }

        public void setDisplayName(String displayName)
        {
            this.displayName = displayName;
        }

        public String getDisplayName()
        {
            return this.displayName;
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType().Equals(this.GetType()))
                return this.id.Equals(((Grantee)obj).id);

            return false;
        }

        public override string ToString()
        {
            return "Grantee [id=" + this.id + ", displayName=" + this.displayName + "]";
        }
    }
}
