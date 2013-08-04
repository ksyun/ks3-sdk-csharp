using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using KS3.Http;
using KS3.Model;

namespace KS3.Internal
{
    public class MetadataResponseHandler : HttpResponseHandler<ObjectMetadata>
    {
        public ObjectMetadata handle(HttpWebResponse response)
        {
            ObjectMetadata metadata = new ObjectMetadata();
            RestUtils.populateObjectMetadata(response, metadata);

            return metadata;
        } 

        public Boolean needsConnectionLeftOpen()
        {
            return false;
        }
    }
}
