using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KS3.Model
{
    public class KS3Object
    {
        /** The key under which this object is stored */
        private String key = null;

        /** The name of the bucket in which this object is contained */
        private String bucketName = null;

        /** The metadata stored by Amazon S3 for this object */
        private ObjectMetadata metadata = new ObjectMetadata();

        /** The stream containing the contents of this object from S3 */
        private Stream objectContent;

        ~KS3Object()
        {
            if (this.objectContent != null) this.objectContent.Close();
        }

        public ObjectMetadata getObjectMetadata()
        {
            return metadata;
        }

        public void setObjectMetadata(ObjectMetadata metadata)
        {
            this.metadata = metadata;
        }

        public Stream getObjectContent()
        {
            return objectContent;
        }

        public void setObjectContent(Stream objectContent)
        {
            this.objectContent = objectContent;
        }

        public String getBucketName()
        {
            return bucketName;
        }

        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

        public String getKey()
        {
            return key;
        }

        public void setKey(String key)
        {
            this.key = key;
        }

        public override String ToString()
        {
            return "S3Object [key=" + getKey()
                + ",bucket=" + (bucketName == null ? "<Unknown>" : bucketName)
                + "]";
        }
    }
}
