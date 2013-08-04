using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class CannedAccessControlList
    {
        public static String PUBLICK_READ_WRITE = "public-read-write";
        public static String PUBLICK_READ = "public-read";
        public static String PRIVATE = "private";

        private String cannedAclHeader;
        
        private Owner ower;

        public CannedAccessControlList() { }

        public CannedAccessControlList(String cannedAclHeader)
        {
            this.cannedAclHeader = cannedAclHeader;
        }

        public String getCannedAclHeader()
        {
            return this.cannedAclHeader;
        }

        public void setCannedAclHeader(String cannedAclHeader)
        {
            this.cannedAclHeader = cannedAclHeader;
        }

        public Owner getOwner()
        {
            return this.ower;
        }

        public void setOwner(Owner owner)
        {
            this.ower = owner;
        }

        public override String ToString()
        {
            return "AccessControlList [owner=" + this.ower + ", cannedAclHeader=" + this.cannedAclHeader + "]";
        }
    }
}
