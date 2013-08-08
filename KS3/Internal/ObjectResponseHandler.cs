﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

using KS3.Http;
using KS3.Model;
using KS3.Internal;

namespace KS3.Internal
{
    public class ObjectResponseHandler : HttpResponseHandler<KS3Object> 
    {
        GetObjectRequest getObjectRequest;
        public ObjectResponseHandler(GetObjectRequest getObjectRequest)
        {
            this.getObjectRequest = getObjectRequest;
        }

        public KS3Object handle(HttpWebResponse response)
        {
            KS3Object ks3Object = new KS3Object();
            FileInfo destinationFile = getObjectRequest.getDestinationFile();
            ProgressListener progressListener = getObjectRequest.getProgressListener();

            ObjectMetadata metadata = new ObjectMetadata();
            RestUtils.populateObjectMetadata(response, metadata);
            ks3Object.setObjectMetadata(metadata);

            Stream input = response.GetResponseStream();

            if (progressListener != null)
                input = new ProgressReportingInputStream(input, progressListener);

            int SIZE = Constants.DEFAULT_STREAM_BUFFER_SIZE;
            byte[] buf = new byte[SIZE];
            byte[] content = null;

            Stream output = null;
            if (destinationFile != null)
                output = getObjectRequest.getDestinationFile().OpenWrite();
            else
            {
                content = new byte[metadata.getContentLength()];
                output = new MemoryStream(content);
            }

            for (; ; )
            {
                int size = input.Read(buf, 0, SIZE);
                if (size <= 0) break;
                output.Write(buf, 0, size);
            }
            input.Close();
            output.Flush();
            output.Close();

            if (destinationFile != null) ks3Object.setObjectContent(destinationFile.OpenRead());
            else ks3Object.setObjectContent(new MemoryStream(content));

            return ks3Object;
        }

        public Boolean needsConnectionLeftOpen()
        {
            return false;
        }
    }
}
