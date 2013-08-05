using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Auth
{
    public class BasicKS3Credentials : KS3Credentials
    {
        private String accessKey;
        private String secretKey;

        public BasicKS3Credentials(String accessKey, String secretKey)
        {
            if (accessKey == null) throw new ArgumentException("Access key cannot be null.");
            if (secretKey == null) throw new ArgumentException("Secret key cannot be null.");
            this.accessKey = accessKey;
            this.secretKey = secretKey;
        }

        public String getKS3AccessKeyId()
        {
            return this.accessKey;
        }

        public String getKS3SecretKey()
        {
            return this.secretKey;
        }
    }
}
