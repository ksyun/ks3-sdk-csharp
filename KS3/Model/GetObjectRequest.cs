using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KS3.Model
{
    public class GetObjectRequest : KS3Request
    {
        /** The name of the bucket containing the object to retrieve */
        private String bucketName;

        /** The key under which the desired object is stored */
        private String key;

        /** Where the content will store */
        private FileInfo destinationFile;

        /** Optional member indicating the byte range of data to retrieve */
        private long[] range;

        /**
         * Optional list of ETag values that constrain this request to only be
         * executed if the object's ETag matches one of the specified ETag values.
         */
        private List<String> matchingETagConstraints = new List<String>();

        /**
         * Optional list of ETag values that constrain this request to only be
         * executed if the object's ETag does not match any of the specified ETag
         * constraint values.
         */
        private List<String> nonmatchingETagContraints = new List<String>();

        /**
         * Optional field that constrains this request to only be executed if the
         * object has not been modified since the specified date.
         */
        private DateTime? unmodifiedSinceConstraint;

        /**
         * Optional field that constrains this request to only be executed if the
         * object has been modified since the specified date.
         */
        private DateTime? modifiedSinceConstraint;

        /**
         * The optional progress listener for receiving updates about object download
         * status.
         */
        private ProgressListener progressListener;


        public GetObjectRequest(String bucketName, String key)
        {
            this.bucketName = bucketName;
            this.key = key;
        }

        public GetObjectRequest(String bucketName, String key, FileInfo destinationFile)
        {
            this.bucketName = bucketName;
            this.key = key;
            this.destinationFile = destinationFile;
        }

        public String getBucketName()
        {
            return this.bucketName;
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

        public FileInfo getDestinationFile()
        {
            return this.destinationFile;
        }

        public void setDestinationFile(FileInfo destinationFile)
        {
            this.destinationFile = destinationFile;
        }

        /**
         * Gets the optional inclusive byte range within the desired object
         * that will be downloaded by this request.
         * The range is returned as
         * a two element array, containing the start and end index of the byte range.
         * If no byte range has been specified, the entire object is downloaded and
         * this method returns <code>null</code>.
         */
        public long[] getRange()
        {
            return range;
        }

        public void setRange(long start, long end)
        {
            this.range = new long[] { start, end };
        }

        public List<String> getMatchingETagConstraints()
        {
            return this.matchingETagConstraints;
        }

        public void setMatchingETagConstraints(List<String> eTagList)
        {
            this.matchingETagConstraints = eTagList;
        }

        public List<String> getNonmatchingETagConstraints()
        {
            return this.nonmatchingETagContraints;
        }

        public void setNonmatchingETagConstraints(List<String> eTagList)
        {
            this.nonmatchingETagContraints = eTagList;
        }

        public DateTime? getUnmodifiedSinceConstraint()
        {
            return this.unmodifiedSinceConstraint;
        }

        public void setUnmodifiedSinceConstraint(DateTime date)
        {
            this.unmodifiedSinceConstraint = date;
        }

        public DateTime? getModifiedSinceConstraint()
        {
            return this.modifiedSinceConstraint;
        }

        public void setModifiedSinceConstraint(DateTime date)
        {
            this.modifiedSinceConstraint = date;
        }

        public void setProgressListener(ProgressListener progressListener)
        {
            this.progressListener = progressListener;
        }

        public ProgressListener getProgressListener()
        {
            return this.progressListener;
        }
    }
}
