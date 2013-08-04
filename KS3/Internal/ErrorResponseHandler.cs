using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using KS3.Transform;
using KS3.Http;

namespace KS3.Internal
{
    public class ErrorResponseHandler : HttpResponseHandler<KS3Exception>
    {
        /** The SAX unmarshaller to use when handling the response from KS3 */
        private ErrorResponseUnmarshaller unmarshaller;

        public ErrorResponseHandler(ErrorResponseUnmarshaller unmarshaller)
        {
            this.unmarshaller = unmarshaller;
        }


        public KS3Exception handle(HttpWebResponse errorResponse)
        {
            KS3Exception ks3Exception = unmarshaller.unmarshall(errorResponse.GetResponseStream());
            ks3Exception.setStatusCode((int)errorResponse.StatusCode);
            return ks3Exception;
        }

        public Boolean needsConnectionLeftOpen()
        {
            return false;
        }
    }
}
